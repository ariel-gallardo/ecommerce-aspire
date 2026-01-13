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
using System.ComponentModel;
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
            new("(NullableOf|DTO|WithChildren|Result|\\d+)", RegexOptions.Compiled);

        private OpenApiSchema HandleSchema(OpenApiSchema schema, bool isProperty = false)
        {
            if (!string.IsNullOrWhiteSpace(schema.Title))
            {
                schema.Title = CleanRegex.Replace(schema.Title, string.Empty)
                    .Replace("IPagedList", "Pagination")
                    .Replace("IResponseOf", "ResponseOf")
                    .Replace("IResponse", "Response");
            }
            
            if (!string.IsNullOrWhiteSpace(schema.Reference?.Id))
            {
                schema.Reference.Id = CleanRegex.Replace(schema.Reference.Id, string.Empty)
                    .Replace("IPagedList", "Pagination")
                    .Replace("IResponseOf", "ResponseOf")
                    .Replace("IResponse", "Response");
            }
            

            if (schema.Annotations?.TryGetValue("x-schema-id", out var raw) == true &&
                raw is string value)
            {
                schema.Annotations["x-schema-id"] = CleanRegex.Replace(value, string.Empty)
                    .Replace("IPagedListOf","Pagination")
                    .Replace("IResponseOf", "ResponseOf")
                    .Replace("IResponse", "Response");
                if (isProperty && schema.Properties.Any() && schema.Type == "object")
                {
                    schema.Properties.Clear();
                    schema.Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = value
                    };
                }
            }
            
            if (schema.Items != null)
                schema.Items = HandleSchema(schema.Items);


            if (schema.Properties != null)
                foreach (var item in schema.Properties)
                    schema.Properties[item.Key] = HandleSchema(item.Value, true);
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

                /*if (operation.Responses.Any())
                {
                    foreach (var item in operation.Responses.First().Value.Content)
                        if(item.Key == "application/json")
                        {
                            operation.Responses.First().Value.Content[item.Key].Schema =
                            HandleSchema(item.Value.Schema);
                        }
                        else operation.Responses.First().Value.Content.Remove(item.Key);
                }*/
                if (responseMethods.Contains(method.Name))
                {
                    var returnsCollection = !excludedPagination.Contains(method.Name) && method.GetParameters().Any(x => (x.ParameterType.IsGenericType && x.ParameterType.GetGenericTypeDefinition() == typeof(IList<>))
                    || (x.ParameterType.BaseType != null && x.ParameterType.BaseType == typeof(QuerieFilter)));


                    if (excludedPagination.Contains(method.Name))
                    {
                        operation.Parameters = operation.Parameters.Where(x => !excludeSingleParameters.Contains(x.Name)).ToList();
                    }

                    var schema = HandleSchema(_schemaGenerator.GenerateSchema(resultDTO, new SchemaRepository()));
                    operation.Responses.Clear();
                    operation.Responses[method.Name == "AddAsync" ? status201String : status200String] = new OpenApiResponse
                    {
                        Content = new Dictionary<string, OpenApiMediaType>()
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    AllOf = new List<OpenApiSchema>
                                    {
                                        new OpenApiSchema
                                        {
                                            Reference = new OpenApiReference
                                            {
                                                Id = "Response",
                                                Type = ReferenceType.Schema
                                            },
                                        },
                                        new OpenApiSchema
                                        {
                                            Properties = new Dictionary<string, OpenApiSchema>
                                            {
                                                ["data"] = new OpenApiSchema
                                                {
                                                    Reference = new OpenApiReference
                                                    {
                                                        Id = returnsCollection ? $"PaginationOf{schema.Reference.Id}" : $"{schema.Reference.Id}",
                                                        Type = ReferenceType.Schema
                                                    }
                                                }
                                            },
                                            Required = new HashSet<string>{"data"}
                                        }
                                    }
                                }
                            }
                        },
                        Description = method.Name == "AddAsync" ? $"Created {schema.Reference.Id}" : "Success",
                    };

                }
                else
                {

                    var schema = HandleSchema(_schemaGenerator.GenerateSchema(typeof(BaseResponse), new SchemaRepository()));
                    schema.Reference.Id = "Response";
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
                    if (operation.RequestBody.Content[key].Schema != null
                        && operation.RequestBody.Content[key].Schema.Annotations != null
                        && operation.RequestBody.Content[key].Schema.Annotations["x-schema-id"] != null)
                    {
                        //operation.RequestBody.Reference = 
                        //operation.RequestBody.Content[key].Schema = null;
                        operation.RequestBody.Content[key].Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Id = (operation.RequestBody.Content[key].Schema.Annotations["x-schema-id"] as string),
                                Type = ReferenceType.Schema
                            }
                        };
                    }
                }
        }
    }

}
