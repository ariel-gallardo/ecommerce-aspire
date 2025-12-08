using Common.Infrastructure.Configurations;
using Common.Infrastructure.Messages.Entities;
using Common.Infrastructure.Persistence;
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
                    var conString = configuration.GetConnectionString("rabbit");
                    cfg.Host(conString);
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
        public static IServiceCollection AddInfrastructure<IDBContext>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env, Assembly[] messageAssemblies) where IDBContext : DbContext
        {
            var name = typeof(IDBContext).Name.Replace("Context", string.Empty);
            
            services.Configure<AppSettings>(options =>
            {
                var currentCfg = configuration.GetSection("Parameters:AppSettings") ?? configuration.GetSection("AppSettings");
                currentCfg.Bind(options);
            });


            services.AddRabbitMq(configuration, env, messageAssemblies);
            services.AddDbContext<DbContext,IDBContext>(options =>
            {
                var sp = services.BuildServiceProvider();
                var conString = $"{configuration.GetConnectionString(name)};Database={name.ToLower()}";
                options
                .UseMySql(conString, ServerVersion.AutoDetect(conString))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(new DbExceptionInterceptor());
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
