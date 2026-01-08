using Common.Application.Contracts;
using Common.Domain.Entities.Base;
using Common.Extensions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Cache.Key;
using Common.Infrastructure.Contracts;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Messages.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Security.Infrastructure.gRPC.Protos;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.RegularExpressions;
using static Security.Infrastructure.gRPC.Protos.PermissionService;

namespace Common.Api.Filters.OpenApi
{
    public class DynamicResponseOperationTransformer : IOpenApiOperationTransformer
    {
        private static string[] excludedPagination = new string[] { "SearchFirstAsync" };
        private static string[] commonMethods = new string[] { "AddAsync", "UpdateAsync", "SearchAsync", "SearchFirstAsync", "UpdateAsync", "DeleteAsync" };
        private static string[] responseMethods = new string[] { "AddAsync", "UpdateAsync", "SearchAsync", "SearchFirstAsync" };
        private static string[] validationMethods = new string[] { "AddAsync", "UpdateAsync" };
        private static string[] excludeSingleParameters = new string[] { "Page", "PageSize" };
        private static string[] collections = new string[] { "", "" };

        private static string status200String = StatusCodes.Status200OK.ToString();
        private static string status201String = StatusCodes.Status201Created.ToString();
        private static string status400String = StatusCodes.Status400BadRequest.ToString();
        private readonly ISchemaGenerator _schemaGenerator;
        private readonly ICommonScopedDataServices _commonData;
        private readonly ICacheManagerServices _cache;
        private readonly PermissionServiceClient _permissionServiceClient;
        private static readonly Regex CleanRegex =
            new("(NullableOf|DTO|PagedList|Result|\\d+)", RegexOptions.Compiled);

        private OpenApiSchema HandleSchema(OpenApiSchema schema)
        {
            if (!string.IsNullOrWhiteSpace(schema.Title))
            {
                schema.Title = CleanRegex.Replace(schema.Title, string.Empty);
            }
            
            if (!string.IsNullOrWhiteSpace(schema.Reference?.Id))
            {
                schema.Reference.Id = CleanRegex.Replace(schema.Reference.Id, string.Empty);
            }
            

            if (schema.Annotations?.TryGetValue("x-schema-id", out var raw) == true &&
                raw is string value)
            {
                schema.Annotations["x-schema-id"] = CleanRegex.Replace(value, string.Empty);
            }
            
            if (schema.Items != null)
                HandleSchema(schema.Items);
            

            return schema;
        }

        private async Task<IList<OpenApiSecurityRequirement>> AssignSecuritySchema(string controller, string action)
        {
            if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                var cacheKey = CacheKeyCommon.PolicyActionName(controller, action);
                string policy = await _cache.GetAsync<string>(cacheKey);
                if (string.IsNullOrEmpty(policy))
                {
                    var data = await _permissionServiceClient.GetPolicyAsync(new PermissionRequest
                    {
                        Action = action,
                        Controller = controller
                    });
                    policy = data.Policy;
                    await _cache.SaveAsync(cacheKey, policy);
                    if (string.IsNullOrEmpty(policy))
                    {
                        data = await _permissionServiceClient.CreatePolicyAsync(new PermissionRequest
                        {
                            Action = action,
                            Controller = controller
                        });
                        policy = data.Policy;
                        await _cache.SaveAsync(cacheKey, policy);
                    }
                }
                var policyEnum = policy.AsEnumUsingMemberValue<Policy>();
                if (policyEnum != Policy.Public && policyEnum != Policy.Unknown)
                {
                    var security = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    };
                    var requirement = new OpenApiSecurityRequirement
                    {
                        [security] = new string[] { }
                    };
                    return new OpenApiSecurityRequirement[] { requirement };
                }
                ;
            }
            return Array.Empty<OpenApiSecurityRequirement>();
        }

        public DynamicResponseOperationTransformer(ISchemaGenerator schemaGenerator, ICommonScopedDataServices commonData,
            PermissionServiceClient permissionServiceClient,
            ICacheManagerServices cache)
        {
            _schemaGenerator = schemaGenerator;
            _commonData = commonData;
            _cache = cache;
            _permissionServiceClient = permissionServiceClient;
        }
        public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var split = context.Description.RelativePath.Split('/');
            var subPath = split.Last();
            var httpMethod = context.Description.HttpMethod;

            if (context.Description.ActionDescriptor is ControllerActionDescriptor cAA)
            {
                if (!operation.Security.Any())
                    operation.Security = await AssignSecuritySchema(cAA.ControllerName, cAA.ActionName);
            }

            if (context.Description.ActionDescriptor is ControllerActionDescriptor cA
                && commonMethods.Contains(cA.MethodInfo.Name)
                && cA.ControllerTypeInfo.BaseType != null
                && cA.ControllerTypeInfo.BaseType is TypeInfo tI
                && tI.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommonController<,,,,,>)))
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
                var (key, domainEntity, addDTO, updateDTO, resultDTO, querieFilter) = (args[0], args[1], args[2], args[3], args[4], args[5]);

                operation.Responses.Clear();
                if (responseMethods.Contains(method.Name))
                {
                    var returnsCollection = !excludedPagination.Contains(method.Name) && method.GetParameters().Any(x => (x.ParameterType.IsGenericType && x.ParameterType.GetGenericTypeDefinition() == typeof(IList<>))
                    || (x.ParameterType.BaseType != null && x.ParameterType.BaseType == typeof(QuerieFilter)));


                    if (excludedPagination.Contains(method.Name))
                    {
                        operation.Parameters = operation.Parameters.Where(x => !excludeSingleParameters.Contains(x.Name)).ToList();
                    }

                    Type cType = returnsCollection ? typeof(PagedList<>).MakeGenericType(resultDTO) : resultDTO;


                    var schema = HandleSchema(_schemaGenerator.GenerateSchema(cType, new SchemaRepository()));

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

                    var schema = HandleSchema(_schemaGenerator.GenerateSchema(typeof(BaseResponse), new SchemaRepository()));
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

                    var schema = HandleSchema(_schemaGenerator.GenerateSchema(typeof(ValidationError), new SchemaRepository()));

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
            else if (
                context.Description.ActionDescriptor is ControllerActionDescriptor cA2
                && cA2.ControllerTypeInfo is TypeInfo tI2
                && tI2.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommonController<,,,,,>)))
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
                        var schema = HandleSchema(_schemaGenerator.GenerateSchema(t, new SchemaRepository()));
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

            if(operation?.RequestBody?.Content != null)
            foreach (var key in operation.RequestBody.Content.Keys)
            {
                operation.RequestBody.Content[key].Schema = HandleSchema(operation.RequestBody.Content[key].Schema);
            }
        }
    }

}
