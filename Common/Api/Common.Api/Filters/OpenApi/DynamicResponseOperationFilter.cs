using Common.Contracts;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Common.Api.Filters.OpenApi
{
    public class DynamicResponseOperationTransformer : IOpenApiOperationTransformer
    {
        private static string[] commonMethods = new string[] { "AddAsync", "UpdateAsync", "SearchAsync", "UpdateAsync", "DeleteAsync" };
        private static string[] responseMethods = new string[] { "AddAsync", "UpdateAsync", "SearchAsync" };
        private static string[] validationMethods = new string[] { "AddAsync", "UpdateAsync" };

        private static string status200String = StatusCodes.Status200OK.ToString();
        private static string status201String = StatusCodes.Status201Created.ToString();
        private static string status400String = StatusCodes.Status400BadRequest.ToString();
        private readonly ISchemaGenerator _schemaGenerator;

        public DynamicResponseOperationTransformer(ISchemaGenerator schemaGenerator)
        {
            _schemaGenerator = schemaGenerator;
        }
        public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            if (context.Description.ActionDescriptor is ControllerActionDescriptor cA
                && commonMethods.Contains(cA.MethodInfo.Name) 
                && cA.ControllerTypeInfo.BaseType != null
                && cA.ControllerTypeInfo.BaseType is TypeInfo tI
                && tI.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommonController<,,,,>)))
            {

                var match = Regex.Match(context.Description.RelativePath, @"\/([^\/]*)$");
                var subPath = match.Groups[2].Value;
                var httpMethod = context.Description.HttpMethod;

                var method = tI.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .FirstOrDefault(m =>
                    {
                        var httpAttr = m.GetCustomAttributes(true).OfType<HttpMethodAttribute>().FirstOrDefault();
                        if (httpAttr == null) return false;
                        bool templateMatch = string.IsNullOrEmpty(subPath)
                            ? httpAttr.Template == null
                            : string.Equals(httpAttr.Template, subPath, StringComparison.OrdinalIgnoreCase);

                        bool methodMatch = httpAttr.HttpMethods.Any(y => string.Equals(y, httpMethod, StringComparison.OrdinalIgnoreCase));
                        return templateMatch && methodMatch;
                    });

                var args = tI.GetGenericArguments();
                var (domainEntity, addDTO, updateDTO, resultDTO, querieFilter) = (args[0], args[1], args[2], args[3], args[4]);

                    operation.Responses.Clear();
                    if (responseMethods.Contains(method.Name))
                    {
                        var responseType = typeof(Response<>).MakeGenericType(resultDTO);
                        var schemaRepository = new SchemaRepository();
                        var schema = _schemaGenerator.GenerateSchema(responseType, schemaRepository);
                        operation.Responses[method.Name == "AddAsync" ? status201String : status200String] = new OpenApiResponse
                        {
                            Description = "Success",
                            Content =
                            {
                                ["application/json"] = new OpenApiMediaType
                                {
                                    Schema = schema,
                                }
                            }
                        };
                    }
                    else
                    {
                        var schemaRepository = new SchemaRepository();
                        var schema = _schemaGenerator.GenerateSchema(typeof(Response), schemaRepository);
                        operation.Responses[status200String] = new OpenApiResponse
                        {
                            Description = "Success",
                            Content =
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = schema,
                            }
                        }
                        };
                    }
                    if (validationMethods.Contains(method.Name))
                    {
                        var schemaRepository = new SchemaRepository();
                        var schema = _schemaGenerator.GenerateSchema(typeof(ValidationError), schemaRepository);

                        var properties = (method.Name == "AddAsync" ? addDTO : updateDTO)
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        var validationErrors = new List<ValidationError>();
                        foreach (var prop in properties)
                            validationErrors.Add(new ValidationError
                            {
                                Property = prop.Name,
                                Message = $"Error message."
                            });

                        var openApiArray = new OpenApiArray();
                        foreach (var error in validationErrors)
                        {
                            var obj = new OpenApiObject
                            {
                                ["property"] = new OpenApiString(error.Property),
                                ["message"] = new OpenApiString(error.Message)
                            };
                            openApiArray.Add(obj);
                        }


                        operation.Responses[status400String] = new OpenApiResponse
                        {
                            Description = "Validation Errors.",
                            Content =
                            {
                                ["application/json"] = new OpenApiMediaType
                                {
                                    Schema = schema,
                                    Example = new OpenApiObject
                                    {
                                        ["statusCode"] = new OpenApiLong(StatusCodes.Status400BadRequest),
                                        ["data"] = openApiArray,
                                        ["errors"] = new OpenApiString("Validation Errors.")
                                    }
                                }
                            }
                        };
                    }
                
            }

        }
    }

}
