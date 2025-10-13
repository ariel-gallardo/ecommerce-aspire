using Common.Domain.DTOS.Base.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Api.Filters.FluentValidation
{
    public class FluentValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var arg in context.ActionArguments)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(arg.Value!.GetType());
                var validator = _serviceProvider.GetService(validatorType) as IValidator;
                if (validator != null)
                {
                    var result = await validator.ValidateAsync(new ValidationContext<object>(arg.Value));
                    if (!result.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(new Response<IEnumerable<ValidationError>>
                        {
                            Data = result.Errors.Select(e => new ValidationError
                            {
                                Property = string.IsNullOrWhiteSpace(e.PropertyName) ? "All" : e.PropertyName,
                                Message = e.ErrorMessage
                            }),
                            Message = "Validation Errors.",
                            StatusCode = StatusCodes.Status400BadRequest
                        });
                        return;
                    }
                }
            }

            await next();
        }
    }

}
