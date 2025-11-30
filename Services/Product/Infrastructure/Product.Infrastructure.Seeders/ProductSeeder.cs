using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;
using ProductEntity = Product.Domain.Entities.Product;

namespace Product.Infrastructure.Seeders
{
    public class ProductSeeder : Seeder, IDevelopmentSeeder
    {
        private IEnumerable<ProductEntity> Products { get; set; }
        public ProductSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
            _dependencies.Add(CacheKeyUser.SeedCreatedIdsAdmin);
            _dependencies.Add(CacheKeyCategory.SeedCreatedIds);
        }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(CacheKeyProduct.SeedIds, CacheKeyProduct.SeedCreatedIds);
            if (await Set<ProductEntity>().AnyAsync(cancellationToken))
            {
                await _cache.SaveAsync(CacheKeyProduct.SeedIds, await Set<ProductEntity>().OrderByDescending(x => x.CreatedAt).Take(_quantity).ProjectTo<Guid>(_mapper.ConfigurationProvider).ToListAsync());
                await _cache.SaveAsync(CacheKeyProduct.SeedCreatedIds, true);
                return Array.Empty<object>();
            }
            await _cache.WaitAsync(_dependencies);
            var userAdminIds = await _cache.GetAsync<List<Guid>>(CacheKeyUser.SeedIdsAdmin);
            var categoryIds = await _cache.GetAsync<List<Guid>>(CacheKeyCategory.SeedIds);

            Products = Enumerable.Range(1, _quantity).Select(i =>
            {
                var entity = new ProductEntity
                {
                    Id = Guid.NewGuid(),
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    CategoryId = categoryIds.ElementAt(_random.Next(categoryIds.Count()-1)),
                    Price = RandomPrice()
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            });
            await _cache.SaveAsync(CacheKeyProduct.SeedIds, Products.Select(x => x.Id));
            await _cache.SaveAsync(CacheKeyProduct.SeedCreatedIds, true);
            return Products;
        }
    }
}
