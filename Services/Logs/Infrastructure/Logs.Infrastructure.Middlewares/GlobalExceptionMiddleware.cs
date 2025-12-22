using Common.Infrastructure.Entities;
using Common.Infrastructure.Messages.Entities;
using Logs.Infrastructure.Messaging.Request;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Logs.Infrastructure.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPublishEndpoint _publisher;

        public GlobalExceptionMiddleware(RequestDelegate next, IPublishEndpoint publisher)
        {
            _next = next;
            _publisher = publisher;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var entryName = Assembly.GetEntryAssembly()?.GetName().Name;
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            await _publisher.Publish(new Message<LogErrorRequest>
            {
                CreatedAt = DateTime.UtcNow.ToString(),
                Data = new LogErrorRequest
                {
                    Exception = exception.ToString(),
                    StackTrace = exception.StackTrace,
                    ExceptionType = exception.GetType().Name,
                    Log = new LogRequest
                    {
                        ServiceName = entryName,
                        Message = exception.Message,
                        TraceId = traceId
                    }
                }
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new BaseResponse
            {
                Message = $"Check TraceId: {traceId}",
                StatusCode = StatusCodes.Status500InternalServerError
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
