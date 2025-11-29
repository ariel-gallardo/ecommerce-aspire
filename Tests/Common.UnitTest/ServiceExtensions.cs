using Common.Infrastructure.Messages.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.UnitTest
{
    public static class ServiceExtensions
    {

        public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = configuration.GetValue<string>("Parameters:AppSettings:Redis:Configuration");
                o.InstanceName = configuration.GetValue<string>("Parameters:AppSettings:Redis:InstanceName");
            });
        }

        public static IServiceCollection ConfigureRabbitMq(this IServiceCollection services, IConfiguration configuration, params Assembly[] rabbitMqAssemblies)
        {
            return services.AddMassTransit(c =>
            {
                var types = rabbitMqAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == typeof(Consumer<>))
                .ToArray();
                c.AddConsumers(types);
                c.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration.GetValue<string>("Parameters:AppSettings:RabbitMQ:Host"));
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }

        public static IServiceCollection ConfigureDatabase<IDBContext>(
            this IServiceCollection services,
            IConfiguration configuration) 
            where IDBContext : DbContext
        {
            var databaseTestingPath = configuration.GetValue<string>(
                "Parameters:AppSettings:DatabaseTestingPath");
            if (!Directory.Exists(databaseTestingPath))
                Directory.CreateDirectory(databaseTestingPath);

            var dbName = typeof(IDBContext).Name.Replace("Context", string.Empty);
            var file = Path.Join(databaseTestingPath, $"{dbName}.sqlite");

            if (File.Exists(file))
                File.Delete(file);

            services.AddDbContext<DbContext,IDBContext>((sp, options) =>
            {
                options.UseSqlite($@"Data Source={file}");
                options.ConfigureWarnings(warnings =>
                    warnings.Ignore(
                        Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
                options.UseSnakeCaseNamingConvention();
            });         

            return services;
        }

    }
}
