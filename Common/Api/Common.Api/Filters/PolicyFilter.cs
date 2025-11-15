using Common.Domain.Exceptions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Cache.Key;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Entities.Const;
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
                    var actionName = CacheKeyCommon.PolicyActionName(controller, action);
                    var actionNameCreated = CacheKeyCommon.PolicyActionNameCreated(controller, action);
                    var policy = await _cache.GetAsync<string>(actionName);
                    if (string.IsNullOrEmpty(policy))
                    {
                        try
                        {
                            _policyClient.Create(new LoadPermissionRequest { Controller = controller, Action = action });
                            await _cache.WaitAsync(actionNameCreated);
                            policy = await _cache.GetAsync<string>(actionName);
                            if (policy == Polices.Public) return;
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
