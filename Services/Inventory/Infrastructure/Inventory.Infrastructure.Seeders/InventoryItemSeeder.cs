using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Cache.Key;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;
using System.Linq;

namespace Inventory.Infrastructure.Seeders
{
    public class InventoryItemSeeder : Seeder, IDevelopmentSeeder
    {
        
        private IEnumerable<InventoryItem> Items { get; set; }
        public InventoryItemSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
            _dependencies.Add(CacheKeyUser.SeedCreatedIdAdmin);
            _dependencies.Add(CacheKeyProduct.SeedCreatedIds);
        }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(
                CacheKeyInventory.SeedUnitProducts,
                CacheKeyInventory.SeedCreatedUnitProducts
            );
            if (await Set<InventoryItem>().AnyAsync(cancellationToken))
            {
                await _cache.SaveAsync(
                    CacheKeyInventory.SeedUnitProducts, 
                    await Set<InventoryItem>()
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(_quantity)
                    .Select(x => new Tuple<Guid, Unit>(x.ProductId, x.Unit)).ToListAsync()
                );
                await _cache.SaveAsync(CacheKeyInventory.SeedCreatedUnitProducts, true);
                return Array.Empty<object>();
            }
            await _cache.WaitAsync(_dependencies, cancellationToken);
            var adminId = await _cache.GetAsync<ulong>(CacheKeyUser.SeedIdAdmin);
            var prodIds = await _cache.GetAsync<List<Guid>>(CacheKeyProduct.SeedIds);
            
            Items = Enumerable.Range(1, prodIds.Count()).Select(i =>
            {
                if (i % 3 == 0) return null;
                var baseUnit = RandomUnitBase();
                var quantity = RandomQuantityByUnitBase(baseUnit);
                var quantityAlert = new Common.Domain.ValueObjects.Quantity { Unit = i%2 == 0 || i % 5 == 0 ? quantity.Unit : baseUnit, Value = quantity.Value * i % 2 == 0 ? 0.15m : i % 5 == 0 ? 1.25m : 5 };
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
            await _cache.SaveAsync(CacheKeyInventory.SeedUnitProducts, Items.Select(x => new Tuple<Guid, Unit>(x.ProductId, x.Unit)));
            await _cache.SaveAsync(CacheKeyInventory.SeedCreatedUnitProducts, true);
            return Items;
        }
    }
}
