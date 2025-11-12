using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Security.Infrastructure.Persistence
{
    public class SecurityDbContext : DbContext
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

          
        }
    }
}
