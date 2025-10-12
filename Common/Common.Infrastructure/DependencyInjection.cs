using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure<IDBContext>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env) where IDBContext : DbContext
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddDbContext<IDBContext>(options =>
            {
                if (env.IsDevelopment())
                {
                    options.UseInMemoryDatabase("InMemoryDB");
                }
            });
            services.AddScoped<DbContext, IDBContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static WebApplication ConfigureDevelopmentSeeders<IDBContext>(this WebApplication app) where IDBContext : DbContext
        {
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<IDBContext>();
                    var userService = services.GetRequiredService<IAuthServices>();
                    var appSettings = services.GetRequiredService<IOptions<AppSettings>>();
                    Seeders.Seed(context, userService, appSettings);
                }
            }
            return app;
        }
    }
}
