using Common.Infrastructure.Entities;
using Logs.Infrastructure.Messaging.Messages.Request;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using IExceptionFilter = Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter;

namespace Common.Api.Filters.Controllers
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IPublishEndpoint _publisher;

        public GlobalExceptionFilter(IPublishEndpoint publisher)
        {
            _publisher = publisher;
        }

        public void OnException(ExceptionContext context)
        {
            var entryName = Assembly.GetEntryAssembly()?.GetName().Name;

            _publisher.Publish(new LogErrorRequest
            {
                Exception = context.Exception.ToString(),
                StackTrace = context.Exception.StackTrace,
                ExceptionType = context.Exception.GetType().Name,
                Log = new LogRequest
                {
                    CreatedAt = DateTime.UtcNow.ToString(),
                    ServiceName = entryName,
                    Message = context.Exception.Message,
                    TraceId = Activity.Current?.Id
                }
            });
            context.Result = new JsonResult(new BaseResponse
            {
                Message = $"Check TraceId: {Activity.Current?.Id}",
                StatusCode = StatusCodes.Status500InternalServerError
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}
