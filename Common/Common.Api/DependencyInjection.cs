using Common.Api.CustomAttributes;
using Common.Api.Filters.FluentValidation;
using Common.Api.Filters.Swagger;
using Common.Api.SwaggerExamples.UserLogin;
using Common.Contracts.DTOS;
using Common.Domain.Contracts.Entities;
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
            services.AddSwaggerGen(c => 
                {
                    c.ExampleFilters();
                    c.SchemaFilter<IgnorePropertiesSwaggerFilter<IAuditableDTO,IgnoreAuditableAttribute>>();
                    c.SchemaFilter<IgnorePropertiesSwaggerFilter<IIdentifiableDTO,IgnoreIdentifiableAttribute>>();
                }
            );
            return services;
        }
    }
}
