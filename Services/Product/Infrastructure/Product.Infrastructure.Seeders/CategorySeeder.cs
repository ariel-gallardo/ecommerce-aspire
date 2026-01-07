

using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Domain.Entities;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;

namespace Product.Infrastructure.Seeders
{
    public class CategorySeeder : Seeder, IDevelopmentSeeder
    {

        private IEnumerable<Category> Categories { get; set; }
        public CategorySeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
        }
        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(CacheKeyCategory.SeedIds, CacheKeyCategory.SeedCreatedIds);
            if (await Set<Category>().AnyAsync(cancellationToken))
            {
                await _cache.SaveAsync(CacheKeyCategory.SeedIds, await Set<Category>().Where(x => !x.Children.Any()).Take(_quantity).ProjectToType<Guid>().ToListAsync());
                await _cache.SaveAsync(CacheKeyCategory.SeedCreatedIds, true);
                return Array.Empty<object>();
            }
            await _cache.WaitAsync(CacheKeyUser.SeedCreatedIdsAdmin, cancellationToken);
            var userAdminIds = await _cache.GetAsync<List<ulong>>(CacheKeyUser.SeedIdsAdmin, cancellationToken);
            int total = _quantity;
            int part = total / 3;
            int remainder = total % 3;

            int aCount = part;
            int bCount = part;
            int cCount = part + remainder;

            var catA = Enumerable.Range(1, aCount).Select(i =>
            {
                var entity = new Category(null)
                {
                    Id = Guid.NewGuid(),
                    Name = $"Category {i}",
                    Description = i % 3 == 0 ? $"Description {i}" : null
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();

            var catB = Enumerable.Range(aCount + 1, bCount).Select(i =>
            {
                var entity = new Category(null)
                {
                    Id = Guid.NewGuid(),
                    Name = $"Category {i}",
                    Description = i % 3 == 0 ? $"Description {i}" : null,
                    ParentId = catA[_random.Next(catA.Count)].Id
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();

            var catC = Enumerable.Range(aCount + bCount + 1, cCount).Select(i =>
            {
                var entity = new Category(null)
                {
                    Id = Guid.NewGuid(),
                    Name = $"Category {i}",
                    Description = i % 3 == 0 ? $"Description {i}" : null,
                    ParentId = catB[_random.Next(catB.Count)].Id
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();

            Categories = catA.Concat(catB).Concat(catC);
            await _cache.SaveAsync(CacheKeyCategory.SeedIds, catC.Select(x => x.Id));
            await _cache.SaveAsync(CacheKeyCategory.SeedCreatedIds, true);
            return Categories;
        }
    }
}
