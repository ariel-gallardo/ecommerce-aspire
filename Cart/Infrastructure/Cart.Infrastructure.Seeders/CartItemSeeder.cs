using Cart.Domain.Entities;
using Cart.Infrastructure.Cache.Key;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Enums;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Contracts;
using Common.Infrastructure.Seeder.Entities;
using Inventory.Infrastructure.Cache.Key;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;
using System.Text.Json;

namespace Cart.Infrastructure.Seeders
{
    public class CartItemSeeder : Seeder, IDevelopmentSeeder
    {
        public CartItemSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IUnitOfWork unitOfWork) : base(options, cache, unitOfWork)
        {
            _dependencies.Add(CacheKeyCart.SeedCreatedIds);
            _dependencies.Add(CacheKeyUser.SeedCreatedIdAdmin);
            _dependencies.Add(CacheKeyCart.SeedCreatedIdsUser);
            _dependencies.Add(CacheKeyInventory.SeedCreatedUnitProducts);
        }

        public IEnumerable<CartItem> Items { get; set; }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            await _cache.RemoveAsync(
                CacheKeyCartItem.SeedIds,
                CacheKeyCartItem.SeedCreatedIds
            );
            await _cache.WaitAsync(_dependencies, cancellationToken);
            var adminId = await _cache.GetAsync<Guid>(CacheKeyUser.SeedIdAdmin);
            var prodIds = await _cache.GetAsync<List<Guid>>(CacheKeyProduct.SeedIds);
            var cartIds = await _cache.GetAsync<List<Guid>>(CacheKeyCart.SeedIds);
            var cartUserIds = await _cache.GetAsync<List<Guid>>(CacheKeyCart.SeedUserIds);
            var unitProductList = await _cache.GetAsync<List<(Guid, Unit)>>(CacheKeyInventory.SeedUnitProducts);

            Items = Enumerable.Range(1, cartIds.Count()).Select(cI =>
            {
                return Enumerable.Range(1, _random.Next(1, prodIds.Count())).Select(pI =>
                {
                    var currentUnit = unitProductList.Where(x => x.Item1 == prodIds.ElementAt(pI - 1)).Select(x => x.Item2).FirstOrDefault();
                    if (currentUnit == null) return null;
                    var unit = RandomUnitByBase(currentUnit);
                    var entity = new CartItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = prodIds.ElementAt(pI - 1),
                        Quantity = new Common.Domain.ValueObjects.Quantity
                        {
                            Value = RandomDecimalByBase(unit),
                            Unit = unit
                        }
                    };
                    AddAuditableProperties(entity, cartUserIds.ElementAt(cI - 1), adminId);
                    return entity;
                }).Where(x => x != null);
            }).SelectMany(x => x);

            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyCartItem.SeedIds, Items.Select(x => x.Id)),
                _unitOfWork.Context.AddRangeAsync(Items, cancellationToken)
            );
            await _cache.SaveAsync(CacheKeyCartItem.SeedCreatedIds, true);
        }
    }
}
