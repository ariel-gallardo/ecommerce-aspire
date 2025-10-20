using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using System.Reflection;

namespace Common.Api
{
    public static class WebApplicationBuilderExtensions
    {
        private static bool _addDefaultAssemblies = false;
        private static Assembly[] _autoMapperAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _validatorAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _serviceAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _seederDevAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _controllerAssemblies = Array.Empty<Assembly>();

        public static WebApplicationBuilder AddDefaultAssemblies(this WebApplicationBuilder builder)
        {
            _addDefaultAssemblies = true;
            return builder;
        }

        public static WebApplicationBuilder AddAutoMapperAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _autoMapperAssemblies = assemblies;
            return builder;
        }

        public static WebApplicationBuilder AddValidatorAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _validatorAssemblies = assemblies;
            return builder;
        }

        public static WebApplicationBuilder AddServiceAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _serviceAssemblies = assemblies;
            return builder;
        }

        public static WebApplicationBuilder AddSeederDevelopmentAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _seederDevAssemblies = assemblies;
            return builder;
        }

        public static WebApplicationBuilder AddControllerAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _controllerAssemblies = assemblies;
            return builder;
        }

        public static WebApplication BuildApi<DBContext>(this WebApplicationBuilder builder)
            where DBContext : DbContext
        {
            var env = builder.Environment;

            // Registramos infraestructura y servicios
            builder.Services.AddInfrastructure<DBContext>(builder.Configuration, env);
            builder.Services.AddApplicationServices(_addDefaultAssemblies, _serviceAssemblies);
            builder.Services.AddApplicationDevelopmentSeeders(env, _addDefaultAssemblies, _seederDevAssemblies);
            builder.Services.AddApplicationAutoMapper(_addDefaultAssemblies, _autoMapperAssemblies);
            builder.Services.AddApplicationValidators(_addDefaultAssemblies, _validatorAssemblies);
            builder.Services.AddApi(_addDefaultAssemblies, _controllerAssemblies);
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(o =>
            {
                
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.MapOpenApi("/openapi/{documentName}/openapi.json");
                using (var scope = app.Services.CreateAsyncScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<SeedersRunner>();
                    seeder.RunAsync();
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
