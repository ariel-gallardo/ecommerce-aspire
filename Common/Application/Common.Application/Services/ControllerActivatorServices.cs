using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Application.Services
{
    public class ControllerActivatorServices : IControllerActivator
    {
        public object Create(ControllerContext actionContext)
        {
            var controllerType = actionContext.ActionDescriptor.ControllerTypeInfo.GetInterfaces().Last();

            return actionContext.HttpContext.RequestServices.GetRequiredService(controllerType);
        }

        public virtual void Release(ControllerContext context, object controller)
        {
        }
    }
}
