using Common.Infrastructure.Configurations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;


namespace Common.Infrastructure
{
    public static class DependencyInjection
    {
        private static IServiceCollection AddRabbitMq(this IServiceCollection services, IHostEnvironment env, Assembly[] messageAssemblies)
        {            
            using (var provider = services.BuildServiceProvider())
            {
                var appSettings = provider.GetRequiredService<IOptions<AppSettings>>()?.Value;
                return services.AddMassTransit(c =>
                {
                    c.AddConsumers(messageAssemblies);
                    c.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(appSettings.RabbitMQ.Host);
                        cfg.UseConcurrencyLimit(appSettings.RabbitMQ.ConcurrenctMessageLimit);
                        cfg.ConcurrentMessageLimit = appSettings.RabbitMQ.ConcurrenctMessageLimit;
                    });
                });
            }
        }
        public static IServiceCollection AddInfrastructure<IDBContext>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env, Assembly[] messageAssemblies) where IDBContext : DbContext
        {
            var name = typeof(IDBContext).Name.Replace("Context", string.Empty);
            
            services.Configure<AppSettings>(options =>
            {
                var currentCfg = configuration.GetSection("Parameters:AppSettings") ?? configuration.GetSection("AppSettings");
                currentCfg.Bind(options);
                options.RabbitMQ.Host = configuration.GetConnectionString("rabbit") ?? $"amqp://{options.RabbitMQ.Username}:{options.RabbitMQ.Password}@localhost:5672";
                options.Redis.Configuration = configuration.GetConnectionString("cache") ?? $"localhost:6379,password={options.Redis.Password}";

            });


            services.AddRabbitMq(env, messageAssemblies);
            services.AddDbContext<DbContext,IDBContext>(options =>
            {
                if (env.IsDevelopment())
                {
                    var sp = services.BuildServiceProvider();
                    var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
                    var file = Path.Join(settings.DatabaseDevPath, $"{name.Replace("Db", string.Empty)}.sqlite");
                    options.UseSqlite(configuration.GetConnectionString(name) ?? $@"Data Source={file}");
                    options.ConfigureWarnings(warnings =>
                    warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
                }
            });
            
            if (env.IsDevelopment())
            {
                var sp = services.BuildServiceProvider();
                var ctx = sp.GetService<DbContext>();
                ctx.Database.Migrate();
            }
            return services;
        }
    }
}
