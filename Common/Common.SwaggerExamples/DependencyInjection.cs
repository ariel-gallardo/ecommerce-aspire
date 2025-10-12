using Common.Api.Filters;
using Common.Api.SwaggerExamples.UserLogin;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddControllers(o =>
            {
                o.Filters.Add<FluentValidationFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddFluentValidationAutoValidation();
            services.AddSwaggerExamplesFromAssemblies(typeof(UserLoginRequestExample).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => { c.ExampleFilters();});
            return services;
        }
    }
}
