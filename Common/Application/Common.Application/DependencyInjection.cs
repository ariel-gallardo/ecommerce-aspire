
using Common.Application.Profiles.Base;
using Common.Application.Services;
using Common.Extensions;
using Common.Infrastructure;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Contracts;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence.Seeds.Base;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Application.gRPC;
using Security.Application.gRPC.Profiles;
using Security.Infrastructure;
using Security.Infrastructure.Entities;
using System.Reflection;
using System.Text;

namespace Common.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationMapper(this IServiceCollection services, IHostEnvironment env, params Assembly[] assemblies)
        {
            var config = new TypeAdapterConfig();
            config.Scan(assemblies.Concat(new[] { typeof(IdentifiableProfile).Assembly, typeof(PermissionProfile).Assembly }).ToArray());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }

        public static IServiceCollection AddApplicationValidators(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddValidatorsFromAssemblies(assemblies);
        }
        public static IServiceCollection AddApplicationRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var sP = services.BuildServiceProvider();
            var appSettings = sP.GetRequiredService<IOptions<AppSettings>>().Value;

            return services.AddStackExchangeRedisCache(options =>
            {              
                var consString = configuration.GetConnectionString("cache");
                options.InstanceName = appSettings.Redis.InstanceName;
                options.Configuration = consString;
            });
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] assemblies) 
        {
            services.AddCors(o =>
            {
                o.AddPolicy("AllowMySite", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Current-Page", "X-Total-Pages", "X-Page-Size", "X-Total-Count", "X-Url");
                });
            });

            services.AddAuthorization(o =>
            {
                o.AddPolicy(Policy.Administrator.AsStringUsingMemberValue(), policy =>
                policy.RequireRole(nameof(Role.Administrator)));

                o.AddPolicy(Policy.Support.AsStringUsingMemberValue(), policy =>
                policy.RequireRole(nameof(Role.Administrator), nameof(Role.Support)));

                o.AddPolicy(Policy.Client.AsStringUsingMemberValue(), policy =>
                policy.RequireRole(nameof(Role.Administrator), nameof(Role.Support), nameof(Role.Client)));
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                var sP = services.BuildServiceProvider();
                var appSettings = sP.GetRequiredService<IOptions<AppSettings>>().Value;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Jwt.Issuer,
                    ValidAudience = appSettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(appSettings.Jwt.Secret)
                    )
                };
            });
            services.AddHttpContextAccessor();
            var allTypes = assemblies.Concat(new[] { typeof(AuthServices).Assembly, 
                typeof(UnitOfWork).Assembly, 
                typeof(CommonServices).Assembly,
                typeof(CacheManagerServices).Assembly
            }).Distinct()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract &&
                   (
                    t.GetInterfaces().Any(st => st.IsAssignableFrom(typeof(IScoped)))
                || t.GetInterfaces().Any(st => st.IsAssignableFrom(typeof(ISingleton)))
                || t.GetInterfaces().Any(st => st.IsAssignableFrom(typeof(ITransient)))
                )
            ).ToList();

            foreach (var type in allTypes)
            {
                var @interface = type.GetInterfaces().Where(i => i != typeof(IScoped) && i != typeof(ISingleton) && i != typeof(ITransient) && i != typeof(ISeeder)).First();
                if (typeof(IScoped).IsAssignableFrom(type)) services.AddScoped(@interface, type);
                else if (typeof(ISingleton).IsAssignableFrom(type)) services.AddSingleton(@interface, type);
                else if (typeof(ITransient).IsAssignableFrom(type)) services.AddTransient(@interface, type);
            }
            services.AddScoped<ICommonServices, CommonServices>();
            services.Decorate<ICommonServices, CommonServicesDecorator>();
            return services;
        }

        public static IServiceCollection AddGrpcServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            var grpcServiceTypes = assemblies.Concat(new[] { typeof(PermissionGrpcService).Assembly })
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    typeof(IGrpcServiceServer).IsAssignableFrom(t)
                )
                .ToList();

            foreach (var grpcServiceType in grpcServiceTypes)
                services = services.AddScoped(grpcServiceType);
            return services;
        }

        public static WebApplication AddGrpcApplication(
            this WebApplication app,
            params Assembly[] assemblies)
        {
            var grpcServiceTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    typeof(IGrpcServiceServer).IsAssignableFrom(t)
                );

            foreach (var serviceType in grpcServiceTypes)
            {
                MapGrpcService(app, serviceType);
            }

            return app;
        }

        private static void MapGrpcService(WebApplication app, Type serviceType)
        {
            var mapMethod = typeof(GrpcEndpointRouteBuilderExtensions)
                .GetMethods()
                .First(m =>
                    m.Name == "MapGrpcService" &&
                    m.IsGenericMethod &&
                    m.GetParameters().Length == 1);

            var genericMethod = mapMethod.MakeGenericMethod(serviceType);
            genericMethod.Invoke(null, new object[] { app });
        }
    }
}
