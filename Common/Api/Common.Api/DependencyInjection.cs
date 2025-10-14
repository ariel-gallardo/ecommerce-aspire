using Common.Api.Controllers;
using Common.Api.CustomAttributes;
using Common.Api.Filters.FluentValidation;
using Common.Api.Filters.Swagger;
using Common.Api.SwaggerExamples.UserLogin;
using Common.Contracts.DTOS;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Common.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services, bool addDefaultAssemblies, Assembly[] controllerAssemblies, Assembly[] swaggerExampleAssemblies)
        {
            var defaultControllerAssemblies = new Assembly[] { typeof(UsersController).Assembly };
            var defaultExampleAssemblies = new Assembly[] { typeof(UserLoginRequestExample).Assembly };

            var ctrl = services.AddControllers(o =>
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

            foreach (var assembly in addDefaultAssemblies ? controllerAssemblies.Concat(defaultControllerAssemblies) : controllerAssemblies)
                ctrl.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));

            services.AddFluentValidationAutoValidation();
            services.AddSwaggerExamplesFromAssemblies(addDefaultAssemblies ? defaultExampleAssemblies.Concat(swaggerExampleAssemblies).ToArray() : swaggerExampleAssemblies);
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
