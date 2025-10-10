using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Impecable.System.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var infraAssembly = Assembly.LoadWithPartialName("Base.Infrastructure");
            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(infraAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
