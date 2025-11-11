using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Seeder.Services
{
    public class SeedersRunner : ISeederRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _context;
        private readonly IEnumerable<Type> _seederTypes;

        public SeedersRunner(IServiceProvider serviceProvider, DbContext context, IEnumerable<Type> seederTypes)
        {
            _serviceProvider = serviceProvider;
            _context = context;
            _seederTypes = seederTypes;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {

            var seedTasks = _seederTypes
                .Select(async seederType =>
                {
                    var seeder = (IDevelopmentSeeder)_serviceProvider.GetRequiredService(seederType);
                    await seeder.SeedAsync(cancellationToken);
                });
            await Task.WhenAll(seedTasks);
            await _context.SaveChangesAsync();
        }
    }

}
