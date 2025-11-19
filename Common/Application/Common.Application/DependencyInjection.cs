using Common.Application.Profiles.Base;
using Common.Application.Services;
using Common.Contracts;
using Common.Extensions;
using Common.Infrastructure;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence.Seeds.Base;
using FluentValidation;
using MassTransit.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Infrastructure;
using Security.Infrastructure.Entities;
using System.Reflection;
using System.Text;

namespace Common.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.ConstructServicesUsing(type => services.BuildServiceProvider().GetService(type));
            }, assemblies.Concat(new[] { typeof(CommonProfile).Assembly }));

            return services;
        }

        public static IServiceCollection AddApplicationValidators(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services.AddValidatorsFromAssemblies(assemblies);
        }
        public static IServiceCollection AddApplicationRedis(this IServiceCollection services)
        {
            var sP = services.BuildServiceProvider();
            var appSettings = sP.GetRequiredService<IOptions<AppSettings>>().Value;

            return services.AddStackExchangeRedisCache(options =>
            {              
                options.InstanceName = appSettings.Redis.InstanceName;
                options.Configuration = appSettings.Redis.Configuration;
            });
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] assemblies) 
        {
            services.AddCors(o =>
            {
                o.AddPolicy("AllowMySite",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
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
            var allTypes = assemblies.Concat(new[] { typeof(AuthServices).Assembly, typeof(UnitOfWork).Assembly, typeof(CommonServices).Assembly }).Distinct()
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
            return services;
        }
    }
}
