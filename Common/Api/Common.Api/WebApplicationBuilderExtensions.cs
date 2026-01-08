using Common.Api.Filters.OpenApi;
using Common.Application;
using Common.Infrastructure;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Seeder;
using Logs.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Security.Infrastructure.gRPC;
using System.Reflection;

namespace Common.Api
{
    public static class WebApplicationBuilderExtensions
    {
        private static Assembly[] _mapperAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _validatorAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _serviceAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _controllerAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _seederAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _messageAssemblies = Array.Empty<Assembly>();
        private static Assembly[] _grpcAssemblies = Array.Empty<Assembly>();

        public static WebApplicationBuilder AddAutoMapperAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _mapperAssemblies = _mapperAssemblies.Concat(assemblies).ToArray();
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

        public static WebApplicationBuilder AddPipelinesAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _serviceAssemblies = _serviceAssemblies.Concat(assemblies).ToArray();
            return builder;
        }

        public static WebApplicationBuilder AddGrpcAssemblies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            _grpcAssemblies = _grpcAssemblies.Concat(assemblies).ToArray();
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
            builder.Services.AddApplicationRedis(builder.Configuration);
            builder.Services.AddApplicationServices(_serviceAssemblies);
            builder.Services.AddGrpcServices(_grpcAssemblies);
            builder.Services.AddApplicationMapper(env,_mapperAssemblies);
            builder.Services.AddApplicationValidators(_validatorAssemblies);
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
                c.UseInlineDefinitionsForEnums();

            })
            .AddSwaggerGenNewtonsoftSupport();
            builder.Services.AddOpenApi(c =>
            {
                c.AddOperationTransformer<DynamicResponseOperationTransformer>();
                c.AddSchemaTransformer<StandardNameSchemaFilter>();
                c.AddDocumentTransformer<DocumentSchemaFilter>();
                c.ShouldInclude = (api) => true;
            });
            builder.Services.AddGrpc();
            builder.AddServiceDefaults();
            builder.AddGrpcSecurityClients();
            var app = builder.Build();
            app.AddGrpcApplication(_grpcAssemblies);
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
