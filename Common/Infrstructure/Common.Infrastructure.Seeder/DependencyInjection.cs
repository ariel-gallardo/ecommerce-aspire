using Common.Infrastructure.Cache;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Contracts;
using Common.Infrastructure.Seeder.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Common.Infrastructure.Seeder
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSeeders(this IServiceCollection services, IHostEnvironment env, params Assembly[] assemblies)
        {
            if (env.IsDevelopment())
            {
                var seederTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var type in seederTypes) services.AddScoped(type);

                var types = assemblies.SelectMany(a => a.GetTypes())
                    .Where(t => typeof(IDevelopmentSeeder).IsAssignableFrom(t) && !t.IsAbstract);
                foreach (var type in types) services.AddScoped(type);
                var prov = services.BuildServiceProvider();
                services.AddKeyedSingleton(typeof(IEnumerable<IDevelopmentSeeder>), "Seeders", types.Select(t => (IDevelopmentSeeder)prov.GetService(t)).ToArray());
                services.AddScoped<ISeederRunner, SeedersRunner>();
                var seeder = services.BuildServiceProvider().GetRequiredService<ISeederRunner>();
                seeder.RunAsync();
            }
            return services;
        }
    }
}
