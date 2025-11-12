using AutoMapper;
using Cart.Domain.Entities;
using Cart.Infrastructure.Cache.Key;
using Common.Domain.Enums;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Inventory.Infrastructure.Cache.Key;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;

namespace Cart.Infrastructure.Seeders
{
    public class CartItemSeeder : Seeder, IDevelopmentSeeder
    {
        public CartItemSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
            _dependencies.Add(CacheKeyCart.SeedCreatedIds);
            _dependencies.Add(CacheKeyUser.SeedCreatedIdAdmin);
            _dependencies.Add(CacheKeyCart.SeedCreatedIdsUser);
            _dependencies.Add(CacheKeyInventory.SeedCreatedUnitProducts);
            _dependencies.Add(CacheKeyProduct.SeedCreatedIds);
        }

        public List<CartItem> Items { get; set; }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(
                CacheKeyCartItem.SeedIds,
                CacheKeyCartItem.SeedCreatedIds
            );
            if(await Set<CartItem>().AnyAsync(cancellationToken))
            {
                await _cache.SaveAsync(CacheKeyCartItem.SeedIds, await Set<CartItem>().OrderByDescending(x => x.CreatedAt).Take(_quantity).ToListAsync());
                await _cache.SaveAsync(CacheKeyCartItem.SeedCreatedIds, true);
                return Array.Empty<object>();
            }
            await _cache.WaitAsync(_dependencies, cancellationToken);
            var adminId = await _cache.GetAsync<Guid>(CacheKeyUser.SeedIdAdmin);
            var prodIds = await _cache.GetAsync<List<Guid>>(CacheKeyProduct.SeedIds);
            var cartIds = await _cache.GetAsync<List<Guid>>(CacheKeyCart.SeedIds);
            var cartUserIds = await _cache.GetAsync<List<Guid>>(CacheKeyCart.SeedUserIds);
            var unitProductList = await _cache.GetAsync<List<Tuple<Guid, Unit>>>(CacheKeyInventory.SeedUnitProducts);

            Items = Enumerable.Range(1, cartIds.Count()).Select(i =>
            {
                return Enumerable.Range(1, _random.Next(1,10)).Select(j =>
                {
                    var prodId = prodIds.ElementAt(_random.Next(prodIds.Count - 1));
                    var currentUnit = unitProductList.Where(x => x.Item1 == prodId).Select(x => x.Item2).FirstOrDefault();
                    if (currentUnit == null) return null;
                    var unit = RandomUnitByBase(currentUnit);
                    var entity = new CartItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = prodId,
                        CartId = cartIds.ElementAt(i-1),
                        Quantity = new Common.Domain.ValueObjects.Quantity
                        {
                            Value = RandomDecimalByBase(unit),
                            Unit = unit
                        }
                    };
                    AddAuditableProperties(entity, cartUserIds.ElementAt(i - 1), adminId);
                    return entity;
                }).DistinctBy(x => x.ProductId);
            }).SelectMany(x => x).Where(x => x != null).ToList();
            await _cache.SaveAsync(CacheKeyCartItem.SeedIds, Items.Select(x => x.Id));
            await _cache.SaveAsync(CacheKeyCartItem.SeedCreatedIds, true);
            return Items;
        }
    }
}
