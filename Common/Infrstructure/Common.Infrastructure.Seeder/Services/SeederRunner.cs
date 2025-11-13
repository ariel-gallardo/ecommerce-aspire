using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.Seeder.Services
{
    public class SeedersRunner : ISeederRunner
    {
        private readonly IEnumerable<IDevelopmentSeeder> _seeders;
        private readonly DbContext _context;
        private readonly ILogger<SeedersRunner> _logger;


        public SeedersRunner(DbContext context, [FromKeyedServices("Seeders")] IEnumerable<IDevelopmentSeeder> seeders, ILogger<SeedersRunner> logger)
        {
            _seeders = seeders;
            _context = context;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {

            var seedTasks = _seeders
                .Select(async seeder =>
                {
                    try
                    {
                        return await seeder.SeedAsync(cancellationToken);
                    }
                    catch(Exception e)
                    {
                        _logger.LogInformation(e, $"Seeder Runner - {_context.Database.ProviderName}");
                        return null;
                    }
                }).ToList();
            var data = (await Task.WhenAll(seedTasks)).Where(x => x != null).SelectMany(x => x).ToList();
            if(data.Count() > 0)
                await _context.AddRangeAsync(data);
            var res = await _context.SaveChangesAsync();
            if (res > 0) _logger.LogInformation($"Seeder Runner - {_context.Database.ProviderName} - New entities from seeds - {res}");
        }
    }

}
