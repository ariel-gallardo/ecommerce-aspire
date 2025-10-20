using Common.Application.Rules;
using Common.Contracts;
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
        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services, bool addDefaultAssemblies, params Assembly[] assemblies)
        {
            var defaultAssemblies = new[] { typeof(Profiles.UserProfile).Assembly };
            services.AddAutoMapper(cfg =>
            {
                cfg.ConstructServicesUsing(type => services.BuildServiceProvider().GetService(type));
            }, addDefaultAssemblies ? defaultAssemblies.Concat(assemblies) : assemblies);

            return services;
        }

        public static IServiceCollection AddApplicationValidators(this IServiceCollection services, bool addDefaultAssemblies, params Assembly[] assemblies)
        {
            var defaultValidatorAssemblies = new[] { typeof(UserLoginDTOValidator).Assembly };
            return services.AddValidatorsFromAssemblies(addDefaultAssemblies ? defaultValidatorAssemblies.Concat(assemblies) : assemblies);
        }
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, bool addDefaultAssemblies, params Assembly[] assemblies) 
        {
            var defaultAssemblies = new Assembly[] { typeof(AuthServices).Assembly, typeof(UserServices).Assembly };
            var currentAssemblies = addDefaultAssemblies ? defaultAssemblies.Concat(assemblies) : defaultAssemblies;
            services.AddAuthorization();
            services.AddAuthentication();
            services.AddHttpContextAccessor();

            var allTypes = currentAssemblies
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

        public static IServiceCollection AddApplicationDevelopmentSeeders(this IServiceCollection services, IHostEnvironment env, bool addDefaultAssemblies, params Assembly[] assemblies)
        {
            if (env.IsDevelopment())
            {
                var defaultAssemblies = new Assembly[] { typeof(UserSeeder).Assembly };
                var currentAssemblies = addDefaultAssemblies ? defaultAssemblies.Concat(assemblies) : assemblies;

                var seederTypes = currentAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var type in seederTypes) services.AddScoped(type);
                
                var types = currentAssemblies.SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types) services.AddScoped(type);

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
