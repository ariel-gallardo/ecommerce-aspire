using Common.Api.Controllers;
using Common.Application.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Common.Api
{
    public static class DependencyInjection
    {
        public static IMvcBuilder AddControllersAsServicesFromDI(
            this IMvcBuilder builder, Assembly[] controllerAssemblies)
        {
            var controllerTypes = controllerAssemblies.SelectMany(x => x.GetTypes()
                .Where(t => !t.IsAbstract && t.IsClass &&
                            t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(CommonController<,,,,>)))
            .Select(t => new { Type = t, GenericType = t.GetInterfaces().Last() })
            .ToArray();

            foreach (var c in controllerTypes)
                builder.Services.AddScoped(c.GenericType, c.Type);

            ArgumentNullException.ThrowIfNull(builder);

            var feature = new ControllerFeature();
            builder.PartManager.PopulateFeature(feature);
            
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            return builder;
        }
        public static IServiceCollection AddApi(this IServiceCollection services, Assembly[] controllerAssemblies)
        {


            services.AddMvc()
            .AddControllersAsServicesFromDI(controllerAssemblies)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ControllerActivatorServices>());


            services.AddFluentValidationAutoValidation();
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
