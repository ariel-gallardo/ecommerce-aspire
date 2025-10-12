using Common.Domain.Contracts.Repositories;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Persistence.Seeds.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
    }
}
