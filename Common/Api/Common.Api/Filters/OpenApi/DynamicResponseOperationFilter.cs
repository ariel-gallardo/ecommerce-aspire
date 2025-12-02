using Common.Application.Contracts;
using Common.Contracts;
using Common.Domain.Entities.Base;
using Common.Extensions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Cache.Key;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Entities.Const;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Messages.Entities;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Security.Infrastructure.Messaging.Messages.Request;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;

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
        private readonly IRequestClient<LoadPermissionRequest> _loadPermissionClient;
        private readonly IRequestClient<CreatePermissionRequest> _createPermissionClient;
        private readonly ICacheManagerServices _cache;

        private async Task<IList<OpenApiSecurityRequirement>> AssignSecuritySchema(string controller, string action)
        {
            if(!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                var cacheKey = CacheKeyCommon.PolicyActionName(controller, action);
                string policy = await _cache.GetAsync<string>(cacheKey);
                if (string.IsNullOrEmpty(policy))
                {
                    MassTransit.Response<Message<string>> message = null;
                    message = await _loadPermissionClient.GetResponse<Message<string>>(new LoadPermissionRequest { Action = action, Controller = controller });
                    policy = message.Message.Data;
                    await _cache.SaveAsync(cacheKey, policy);
                    if (string.IsNullOrEmpty(policy))
                    {
                        message = await _createPermissionClient.GetResponse<Message<string>>(new CreatePermissionRequest { Action = action, Controller = controller });
                        policy = message.Message.Data;
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
                };
            }
            return Array.Empty<OpenApiSecurityRequirement>();
        }

        public DynamicResponseOperationTransformer(ISchemaGenerator schemaGenerator, ICommonScopedDataServices commonData, 
            IRequestClient<LoadPermissionRequest> loadPermissionClient, 
            IRequestClient<CreatePermissionRequest> createPermissionClient,
            ICacheManagerServices cache)
        {
            _schemaGenerator = schemaGenerator;
            _commonData = commonData;
            _loadPermissionClient = loadPermissionClient;
            _createPermissionClient = createPermissionClient;
            _cache = cache;
        }
        public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            
            var split = context.Description.RelativePath.Split('/');
            var subPath = split.Last();
            var httpMethod = context.Description.HttpMethod;

            if(context.Description.ActionDescriptor is ControllerActionDescriptor cAA)
            {
                if (!operation.Security.Any())
                    operation.Security = await AssignSecuritySchema(cAA.ControllerName, cAA.ActionName);
            }

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
                        
                        var returnsCollection = !excludedPagination.Contains(method.Name) && method.GetParameters().Any(x => (x.ParameterType.IsGenericType && x.ParameterType.GetGenericTypeDefinition() == typeof(IList<>))
                        || (x.ParameterType.BaseType != null && x.ParameterType.BaseType == typeof(QuerieFilter)));
                       
                        var schema = _schemaGenerator.GenerateSchema(returnsCollection ? typeof(PagedList<>).MakeGenericType(resultDTO) : resultDTO, schemaRepository);
                        if (excludedPagination.Contains(method.Name))
                        {
                            operation.Parameters = operation.Parameters.Where(x => !excludeSingleParameters.Contains(x.Name)).ToList();
                        }

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

                var headMethod = tI2.GetMethods()
                .Where(m => m.GetCustomAttributes(true)
                    .OfType<HttpMethodAttribute>()
                    .Any(a => a.HttpMethods.Contains("HEAD")))
                .FirstOrDefault();

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
