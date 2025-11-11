using Common.Infrastructure.Configurations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace Common.Infrastructure
{
    public static class DependencyInjection
    {
        private static IServiceCollection AddRabbitMq(this IServiceCollection services, IHostEnvironment env)
        {            
            using (var provider = services.BuildServiceProvider())
            {
                var appSettings = provider.GetRequiredService<IOptions<AppSettings>>()?.Value;
                return services.AddMassTransit(c =>
                {
                    c.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(appSettings.RabbitMQ.Host);
                    });
                });
            }
        }
        public static IServiceCollection AddInfrastructure<IDBContext>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env) where IDBContext : DbContext
        {
            services.Configure<AppSettings>(options =>
            {
                var currentCfg = configuration.GetSection("Parameters:AppSettings") ?? configuration.GetSection("AppSettings");
                currentCfg.Bind(options);
                options.RabbitMQ.Host = configuration.GetConnectionString("rabbit") ?? $"amqp://{options.RabbitMQ.Username}:{options.RabbitMQ.Password}@localhost:5672";
                options.Redis.Configuration = configuration.GetConnectionString("cache") ?? options.Redis.Configuration;
            });

            services.AddRabbitMq(env);
            services.AddDbContext<IDBContext>(options =>
            {
                if (env.IsDevelopment())
                {
                    options.UseSqlite(configuration.GetConnectionString($"{typeof(IDBContext).Name.Replace("Context", string.Empty)}"));
                }
            });
            services.AddScoped<DbContext, IDBContext>();
            return services;
        }
    }
}
