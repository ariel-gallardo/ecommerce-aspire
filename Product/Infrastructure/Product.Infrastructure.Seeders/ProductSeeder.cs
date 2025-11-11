using Common.Domain.Contracts.Repositories;
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
        public ProductSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IUnitOfWork unitOfWork) : base(options, cache, unitOfWork)
        {
            _dependencies.Add(CacheKeyUser.SeedCreatedIdsAdmin);
            _dependencies.Add(CacheKeyCategory.SeedCreatedIds);
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
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

            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyProduct.SeedIds, Products.Select(x => x.Id)),
                _unitOfWork.Context.AddRangeAsync(Products, cancellationToken)
            );
            await _cache.SaveAsync(CacheKeyProduct.SeedCreatedIds, true);
        }
    }
}
