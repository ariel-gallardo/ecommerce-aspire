using Common.Api.Filters.OpenApi;
using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Seeder;
using Logs.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Common.Api
{
    public static class WebApplicationBuilderExtensions
    {
        private static Assembly[] _autoMapperAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _validatorAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _serviceAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _controllerAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _seederAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _messageAssemblies = Array.Empty<Assembly>();

        public static WebApplicationBuilder AddAutoMapperAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _autoMapperAssemblies = _autoMapperAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddValidatorAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _validatorAssemblies = _validatorAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddServiceAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _serviceAssemblies = _serviceAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddControllerAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _controllerAssemblies = _controllerAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddSeederAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _seederAssemblies = _seederAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddMessageAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _messageAssemblies = _messageAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddQuerieModifierAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _serviceAssemblies = _serviceAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplication BuildApi<DBContext>(this WebApplicationBuilder builder)
            where DBContext : DbContext
        {
            var env = builder.Environment;
            if (env.IsDevelopment() || env.IsEnvironment("Testing"))
            {
                var apiAssembly = Assembly.GetExecutingAssembly();
                builder.Configuration.AddUserSecrets(apiAssembly);
            }

            
            builder.Services.AddInfrastructure<DBContext>(builder.Configuration, env,_messageAssemblies);
            builder.Services.AddApplicationServices(_serviceAssemblies);
            builder.Services.AddApplicationAutoMapper(env,_autoMapperAssemblies);
            builder.Services.AddApplicationValidators(_validatorAssemblies);
            builder.Services.AddApplicationRedis(builder.Configuration);
            builder.Services.AddSeeders(env, _seederAssemblies);
            builder.Services.AddApi(_controllerAssemblies);
            builder.Services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    return new[] {
                        "GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS", "HEAD"
                    }.Contains(apiDesc.HttpMethod);
                });
            });
            builder.Services.AddOpenApi(c =>
            {
                c.AddOperationTransformer<DynamicResponseOperationTransformer>();
                c.ShouldInclude = (api) => true;
            });
            builder.AddServiceDefaults();
            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseCors("AllowMySite");
            app.MapDefaultEndpoints();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi("/swagger/docs/{documentName}/swagger.json");
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
