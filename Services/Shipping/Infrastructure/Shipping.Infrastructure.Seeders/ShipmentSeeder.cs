using AutoMapper;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.Extensions.Options;

namespace Shipping.Infrastructure.Seeders
{
    public class ShipmentSeeder : Seeder, IDevelopmentSeeder
    {
        public ShipmentSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
        }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            return Array.Empty<object>();
        }
    }
}
