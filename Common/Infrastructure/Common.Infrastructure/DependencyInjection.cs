using Common.Infrastructure.Configurations;
using Common.Infrastructure.Messages.Entities;
using Logs.Infrastructure.Messaging.Consumer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Common.Infrastructure
{
    public static class DependencyInjection
    {
        private static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env, Assembly[] messageAssemblies)
        {
            return services.AddMassTransit(c =>
            {
                var types = messageAssemblies
                .Concat(new Assembly[] { typeof(LogErrorRequestConsumer).Assembly })
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == typeof(Consumer<>))
                .ToArray();
                c.AddConsumers(types);
                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("rabbit"));
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
        public static IServiceCollection AddInfrastructure<IDBContext>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env, Assembly[] messageAssemblies) where IDBContext : DbContext
        {
            var name = typeof(IDBContext).Name.Replace("Context", string.Empty);
            
            services.Configure<AppSettings>(options =>
            {
                var currentCfg = configuration.GetSection("AppSettings") ?? configuration.GetSection("Parameters:AppSettings");
                currentCfg.Bind(options);
            });


            services.AddRabbitMq(configuration, env, messageAssemblies);
            services.AddDbContext<DbContext,IDBContext>(options =>
            {
                var conString = $"{configuration.GetConnectionString(name)};Database={name.ToLower()}";
                options.UseMySql(conString, ServerVersion.AutoDetect(conString));
                options.UseSnakeCaseNamingConvention();
            });
            
            if (env.IsDevelopment())
            {
                var sp = services.BuildServiceProvider();
                var ctx = sp.GetService<DbContext>();
                ctx.Database.EnsureCreated();
                if (ctx.Database.GetPendingMigrations().Any())
                {
                    ctx.Database.MigrateAsync();
                }
            }
            return services;
        }
    }
}
