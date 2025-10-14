using AutoMapper;
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
        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton(provider =>
            {
                using (var scope = provider.CreateScope())
                {
                    var profileTypes = new[] { typeof(Profiles.UserProfile).Assembly }.Concat(assemblies)
                        .SelectMany(a => a.GetTypes())
                        .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract);
                    var config = new MapperConfiguration(cfg =>
                    {
                        foreach (var type in profileTypes)
                        {
                            var constructors = type.GetConstructors();
                            var constructor = constructors.FirstOrDefault();
                            if (constructor != null)
                            {

                                var parameters = constructor.GetParameters()
                                .Select(p => scope.ServiceProvider.GetRequiredService(p.ParameterType))
                                .ToArray();

                                var profile = (Profile)Activator.CreateInstance(type, parameters);
                                cfg.AddProfile(profile);
                            }
                        }
                    });
                    return config.CreateMapper();
                }
            });

            return services;
        }

        public static IServiceCollection AddApplicationValidators(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddValidatorsFromAssemblies(new[] { typeof(UserLoginDTOValidator).Assembly }.Concat(assemblies));
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, params Assembly[] assemblies) 
        {
            services.AddHttpContextAccessor();

            var allTypes = assemblies.Concat(new Assembly[] { typeof(AuthServices).Assembly, typeof(UserServices).Assembly })
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
