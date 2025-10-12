using Application.Contracts.Services;
using Common.Application.Rules;
using Common.Domain.Contracts.Services;
using Common.Infrastructure;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
