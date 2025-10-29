using Common.Application.Contracts;
using Common.Contracts;
using Common.Domain.Entities.Base;
using Common.Extensions;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

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
        private readonly ICommonScopedDataServices _commonData;

        public DynamicResponseOperationTransformer(ISchemaGenerator schemaGenerator, ICommonScopedDataServices commonData)
        {
            _schemaGenerator = schemaGenerator;
            _commonData = commonData;
        }
        public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var split = context.Description.RelativePath.Split('/');
            var subPath = split.Last();
            var httpMethod = context.Description.HttpMethod;

            if (context.Description.ActionDescriptor is ControllerActionDescriptor cA
                && commonMethods.Contains(cA.MethodInfo.Name) 
                && cA.ControllerTypeInfo.BaseType != null
                && cA.ControllerTypeInfo.BaseType is TypeInfo tI
                && tI.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommonController<,,,,>)))
            {
                    var method = tI.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .FirstOrDefault(m =>
                    {
                        if (_commonData.ProcessedMethods.Contains(m)) return false;
                        var httpAttr = m.GetCustomAttributes(true).OfType<HttpMethodAttribute>().FirstOrDefault();
                        if (httpAttr == null) return false;
                        bool templateMatch = subPath == cA.ControllerName
                            ? httpAttr.Template == null
                            : string.Equals(httpAttr.Template, subPath, StringComparison.OrdinalIgnoreCase);

                        bool methodMatch = httpAttr.HttpMethods.Any(y => string.Equals(y, httpMethod, StringComparison.OrdinalIgnoreCase));
                        return templateMatch && methodMatch;
                    });
                    _commonData.ProcessedMethods.Add(method);

                    var args = tI.GetGenericArguments();
                    var (domainEntity, addDTO, updateDTO, resultDTO, querieFilter) = (args[0], args[1], args[2], args[3], args[4]);

                    operation.Responses.Clear();
                    if (responseMethods.Contains(method.Name))
                    {
                        var schemaRepository = new SchemaRepository();

                        var returnsCollection = method.GetParameters().Any(x => (x.ParameterType.IsGenericType && x.ParameterType.GetGenericTypeDefinition() == typeof(IList<>))
                        || (x.ParameterType.BaseType != null && x.ParameterType.BaseType == typeof(QuerieFilter)));
                       
                        var schema = _schemaGenerator.GenerateSchema(returnsCollection ? typeof(PagedList<>).MakeGenericType(resultDTO) : resultDTO, schemaRepository);
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
                        var schema = _schemaGenerator.GenerateSchema(typeof(BaseResponse), schemaRepository);
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
                                Property = prop.Name.ToCamelCase(),
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
            else if(
                context.Description.ActionDescriptor is ControllerActionDescriptor cA2
                && cA2.ControllerTypeInfo is TypeInfo tI2
                && tI2.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommonController<,,,,>)))
            {
                var method = tI2.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    if (_commonData.ProcessedMethods.Contains(m)) return false;
                    var httpAttr = m.GetCustomAttributes(true).OfType<HttpMethodAttribute>().FirstOrDefault();
                    if (httpAttr == null) return false;
                    bool templateMatch = subPath == cA2.ControllerName
                        ? httpAttr.Template == null
                        : string.Equals(httpAttr.Template, subPath, StringComparison.OrdinalIgnoreCase);

                    bool methodMatch = httpAttr.HttpMethods.Any(y => string.Equals(y, httpMethod, StringComparison.OrdinalIgnoreCase));
                    return templateMatch && methodMatch;
                });
                _commonData.ProcessedMethods.Add(method);
                var status200Response = method.CustomAttributes
                    .FirstOrDefault(attr => attr.AttributeType == typeof(ProducesResponseTypeAttribute) &&
                                            attr.ConstructorArguments.Any(arg => (int)arg.Value == StatusCodes.Status200OK));

                var status201Response = method.CustomAttributes
                     .FirstOrDefault(attr => attr.AttributeType == typeof(ProducesResponseTypeAttribute) &&
                            attr.ConstructorArguments.Any(arg => (int)arg.Value == StatusCodes.Status201Created));

                if (status200Response != null || status201Response != null)
                {

                    var typeMember = (status200Response != null ? status200Response : status201Response).NamedArguments
                        .FirstOrDefault(x => x.MemberName == "Type");

                    if (typeMember != null && typeMember.TypedValue.Value is Type t)
                    {
                        var schemaRepository = new SchemaRepository();
                        var schema = _schemaGenerator.GenerateSchema(t, schemaRepository);
                        operation.Responses[status200Response != null ? status200String : status201String] = new OpenApiResponse
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
                }
            }
        }
    }

}
