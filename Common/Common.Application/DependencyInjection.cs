using Application.Contracts.Services;
using Common.Application.Rules;
using Common.Domain.Contracts.Services;
using Common.Infrastructure;
using Common.Infrastructure.Persistence.Seeds;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Persistence.Seeds.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Common.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddAutoMapper(new[]{typeof(Base.Application.Profiles.UserProfile).Assembly}.Concat(assemblies));
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddValidatorsFromAssemblies(new[] { typeof(UserLoginDTOValidator).Assembly }.Concat(assemblies));
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] assemblies) 
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IAuthServices, AuthServices>();

            var allTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToList();

            foreach (var type in allTypes)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i != typeof(IScoped) && i != typeof(ISingleton) && i != typeof(ITransient))
                    .ToList();
                if (typeof(IScoped).IsAssignableFrom(type))
                {
                    foreach (var i in interfaces)
                        services.AddScoped(i, type);
                }
                else if (typeof(ISingleton).IsAssignableFrom(type))
                {
                    foreach (var i in interfaces)
                        services.AddSingleton(i, type);
                }
                else if (typeof(ITransient).IsAssignableFrom(type))
                {
                    foreach (var i in interfaces)
                        services.AddTransient(i, type);
                }
            }
            return services;
        }

        public static IServiceCollection AddApplicationDevelopmentSeeders(this IServiceCollection services, IHostEnvironment env, params Assembly[] assemblies)
        {
            if (env.IsDevelopment())
            {
                var seederTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var type in seederTypes) services.AddScoped(type);
                
                var types = new Assembly[] { typeof(UserSeeder).Assembly }.Concat(assemblies).SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types)
                    services.AddScoped(type);

                services.AddScoped(sp =>
                {
                    var context = sp.GetRequiredService<DbContext>();
                    return new SeedersRunner(sp, context, types);
                });
            }
            return services;
        }
    }
}
