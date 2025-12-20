

using Common.Domain.Entities;
using Common.Domain.ValueObjects;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Infrastructure.Cache.Key;

namespace Security.Infrastructure.Seeders
{
    public class PersonaSeeder : Seeder, IDevelopmentSeeder
    {
        private IEnumerable<Persona> People { get; set; }

        public PersonaSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
            People = Array.Empty<Persona>();
            _dependencies.Add(CacheKeyUser.SeedCreatedIdsAdmin);
        }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(
                CacheKeyPersona.SeedIds,
                CacheKeyPersona.SeedCreatedIds
            );
            await _cache.WaitAsync(_dependencies, cancellationToken);
            if (await Set<Persona>().AnyAsync(cancellationToken))
            {
                await _cache.SaveAsync(CacheKeyPersona.SeedIds, await Set<Persona>().OrderByDescending(x => x.CreatedAt).ProjectToType<Guid>().ToListAsync(cancellationToken));
                await _cache.SaveAsync(CacheKeyPersona.SeedCreatedIds, true);
                return Array.Empty<object>();
            }
            
            var userAdminIds = await _cache.GetAsync<List<ulong>>(CacheKeyUser.SeedIdsAdmin);

            People = Enumerable.Range(1, _quantity).Select(x =>
            {
                var entity = new Persona
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                    Name = $"Persona Name {x}",
                    Lastname = $"Persona LastName {x}",
                    Address = new Address
                    {
                        Coordinates = new Coordinates
                        {
                            Latitude = Decimal.Parse($"{_random.NextDouble() * 180 - 90}"),
                            Longitude = Decimal.Parse($"{_random.NextDouble() * 360 - 180}")
                        },
                        Description = $"Address Description {x}",
                        Neighborhood = $"Address Neighborhood {x}",
                        Number = _random.Next(1, 100),
                        Street = $"Address Street {x}"
                    }
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();
            await _cache.SaveAsync(CacheKeyPersona.SeedIds, People.Select(x => x.Id));
            await _cache.SaveAsync(CacheKeyPersona.SeedCreatedIds, true);
            return People;
        }
    }
}
