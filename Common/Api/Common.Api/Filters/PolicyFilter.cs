using Common.Extensions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Cache.Key;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Entities.Const;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Messages.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Security.Infrastructure.Contracts;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Common.Api.Filters
{
    public class PolicyFilter : IAsyncAuthorizationFilter
    {
        private readonly ICacheManagerServices _cache;
        private readonly IRequestClient<LoadPermissionRequest> _policyClient;
        private readonly IAuthServices _authServices;

        public PolicyFilter(ICacheManagerServices cache, IRequestClient<LoadPermissionRequest> policyClient, IAuthServices authServices)
        {
            _cache = cache;
            _policyClient = policyClient;
            _authServices = authServices;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var controller = actionDescriptor?.ControllerName;
            var action = actionDescriptor?.ActionName;
            if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                string policy = Policy.Unknown.AsStringUsingMemberValue();
                var actionName = CacheKeyCommon.PolicyActionName(controller, action);
                var actionNameCreated = CacheKeyCommon.PolicyActionNameCreated(controller, action);
                var policyUrl = string.Empty;
                var policyUrlKey = string.Empty;
                if (action == "CanAccess" && controller == "Permission")
                {
                    if (context.HttpContext.Request.Headers.TryGetValue("X-Url", out var value))
                    {
                        var url = value.FirstOrDefault();
                        if (!String.IsNullOrWhiteSpace(url))
                        {
                            policyUrl = url;
                            policyUrlKey = CacheKeyCommon.PolicyUrl(url);
                            policy = await _cache.GetAsync<string>(policyUrlKey);
                            if (string.IsNullOrWhiteSpace(policy)) policy = Policy.Unknown.AsStringUsingMemberValue();
                        }
                    }
                }
                else
                {
                    policy = await _cache.GetAsync<string>(actionName);
                    if (string.IsNullOrWhiteSpace(policy)) policy = Policy.Unknown.AsStringUsingMemberValue();
                }
                if (policy.AsEnumUsingMemberValue<Policy>() == Policy.Unknown)
                {
                    try
                    {
                        var response = await _policyClient.GetResponse<Message<string>>(
                            String.IsNullOrWhiteSpace(policyUrl) ?
                            new LoadPermissionRequest { Controller = controller, Action = action }
                            : new LoadPermissionRequest { Url = policyUrl }
                            );
                        var data = response.Message.Data;
                        if (!String.IsNullOrWhiteSpace(data))
                        {
                            policy = data;
                            if (!string.IsNullOrWhiteSpace(policyUrl))
                            {
                                await _cache.SaveAsync(policyUrlKey, policy);
                            }
                            else if (!string.IsNullOrWhiteSpace(action))
                            {
                                await _cache.SaveAsync(actionName, policy);
                                await _cache.SaveAsync(actionNameCreated, true);
                                if (policy.AsEnumUsingMemberValue<Policy>() == Policy.Public) return;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (!_authServices.IsAuthenticated)
                        {
                            var response = new ObjectResult(new BaseResponse
                            {
                                StatusCode = StatusCodes.Status401Unauthorized,
                                Message = "Unauthorized."
                            });
                            response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Result = response;
                            return;
                        }
                        var result = new ObjectResult(new BaseResponse
                        {
                            Message = e.Message,
                            StatusCode = StatusCodes.Status403Forbidden
                        });
                        result.StatusCode = StatusCodes.Status403Forbidden;
                        context.Result = result;
                        return;
                    }
                }
                if (policy.AsEnumUsingMemberValue<Policy>() == Policy.Unknown)
                {
                    var response = new ObjectResult(new BaseResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Pardon our dust! This page is currently under development."
                    });
                    response.StatusCode = StatusCodes.Status404NotFound;
                    context.Result = response;
                    return;
                }
                else
                {
                    var canAccess = await _authServices.CanAccess(policy);
                    if (!canAccess.HasValue || !canAccess.Value)
                    {
                        var response = new ObjectResult(new BaseResponse
                        {
                            StatusCode = canAccess == null
                            ? StatusCodes.Status401Unauthorized : StatusCodes.Status403Forbidden,
                            Message = canAccess == null ? "Unauthorized." : "You do not have permission to perform this action."
                        });
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Result = response;
                        return;
                    }
                }
            }

        }
    }
}
