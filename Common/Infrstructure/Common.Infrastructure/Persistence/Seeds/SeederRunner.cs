using Common.Infrastructure.Persistence.Seeds.Base;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Persistence.Seeds
{
    public class SeedersRunner
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

        public async Task RunAsync()
        {
            foreach (var seederType in _seederTypes)
            {
                var seeder = (IDevelopmentSeeder)_serviceProvider.GetService(seederType);
                await seeder.SeedAsync(_context);
            }
            await _context.SaveChangesAsync();
        }
    }

}
