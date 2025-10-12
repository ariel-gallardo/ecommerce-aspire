using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Persistence.Seeds;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Persistence.Seeds.Entities;
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
        private static Assembly[] _seederAssemblies = Array.Empty<Assembly>();
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
            if (builder.Environment.IsDevelopment())
            {
                _seederAssemblies = assemblies;

                var seederTypes = assemblies
                    .SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract).ToList();
                foreach (var type in seederTypes)
                {
                    builder.Services.AddScoped(type);
                }
                var types = new Assembly[] { typeof(UserSeeder).Assembly }.Concat(assemblies).SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types)
                    builder.Services.AddScoped(type);

                builder.Services.AddScoped(sp =>
                {
                    var context = sp.GetRequiredService<DbContext>();
                    return new SeedersRunner(sp, context, types);
                });
            }
            return builder;
        }
        public static WebApplication BuildApi<DBContext>(this WebApplicationBuilder builder)  where DBContext : DbContext
        {
            var env = builder.Environment;

            builder.Services.AddInfrastructure<DBContext>(builder.Configuration, env);
            builder.Services.AddApplicationAutoMapper(_autoMapperAssemblies);
            builder.Services.AddApplicationValidators(_validatorAssemblies);
            builder.Services.AddApplicationServices(_serviceAssemblies);
            builder.Services.AddApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var seedRunner = scope.ServiceProvider.GetRequiredService<SeedersRunner>();
                seedRunner.RunAsync().GetAwaiter().GetResult();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
