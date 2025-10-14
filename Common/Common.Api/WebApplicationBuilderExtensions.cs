using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Persistence.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Common.Api
{
    public static class WebApplicationBuilderExtensions
    {
        private static Assembly[] _autoMapperAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _validatorAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _serviceAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _seederDevAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _jsonConverterAssemblies = Array.Empty<Assembly>();
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

        public static WebApplicationBuilder AddJsonConverterAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _serviceAssemblies = assemblies;
            return builder;
        }

        public static WebApplicationBuilder AddSeederDevelopmentAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _seederDevAssemblies = assemblies;
            return builder;
        }

        private static void UseSwaggerIfDevelopment(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        public static WebApplication BuildApi<DBContext>(this WebApplicationBuilder builder)  where DBContext : DbContext
        {
            var env = builder.Environment;
            builder.Services.AddInfrastructure<DBContext>(builder.Configuration, env);
            builder.Services.AddApplicationServices(_serviceAssemblies);
            builder.Services.AddApplicationDevelopmentSeeders(env, _seederDevAssemblies);
            builder.Services.AddApplicationAutoMapper(_autoMapperAssemblies);
            builder.Services.AddApplicationValidators(_validatorAssemblies);
            builder.Services.AddApi();
            var app = builder.Build();
            app.UseSwaggerIfDevelopment();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
