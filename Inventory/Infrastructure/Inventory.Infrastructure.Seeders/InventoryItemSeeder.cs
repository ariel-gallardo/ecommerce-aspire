using Common.Domain.Contracts.Repositories;
using Common.Domain.Enums;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Contracts;
using Common.Infrastructure.Seeder.Entities;
using Inventory.Domain;
using Inventory.Infrastructure.Cache.Key;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;
using System.Text.Json;

namespace Inventory.Infrastructure.Seeders
{
    public class InventoryItemSeeder : Seeder, IDevelopmentSeeder
    {
        
        private IEnumerable<InventoryItem> Items { get; set; }
        public InventoryItemSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IUnitOfWork unitOfWork) : base(options, cache, unitOfWork)
        {
            _dependencies.Add(CacheKeyUser.SeedCreatedIdAdmin);
            _dependencies.Add(CacheKeyProduct.SeedCreatedIds);
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            await _cache.RemoveAsync(
                CacheKeyInventory.SeedUnitProducts,
                CacheKeyInventory.SeedCreatedUnitProducts
            );
            await _cache.WaitAsync(_dependencies, cancellationToken);
            var adminId = await _cache.GetAsync<Guid>(CacheKeyUser.SeedIdAdmin);
            var prodIds = await _cache.GetAsync<List<Guid>>(CacheKeyProduct.SeedIds);
            
            Items = Enumerable.Range(1, prodIds.Count()).Select(i =>
            {
                if (i % 3 == 0) return null;
                var baseUnit = RandomUnitBase();
                var quantity = RandomQuantityByUnitBase(baseUnit);
                var quantityAlert = quantity;
                quantityAlert.Value = quantityAlert.Value * 0.15m;
                var entity = new InventoryItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = prodIds.ElementAt(i-1),
                    Unit = baseUnit,
                    Quantity = quantity,
                    QuantityAlert = quantityAlert,
                };
                AddAuditableProperties(entity, adminId, adminId);
                return entity;
            }).Where(x => x != null);

            await Task.WhenAll(
                _unitOfWork.Context.AddRangeAsync(Items),
                _cache.SaveAsync(CacheKeyInventory.SeedUnitProducts, Items.Select(x => new Tuple<Guid, Unit>(x.ProductId, x.Unit)))
            );
            await _cache.SaveAsync(CacheKeyInventory.SeedCreatedUnitProducts, true);
        }
    }
}
