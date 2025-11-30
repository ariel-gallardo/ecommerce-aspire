using AutoMapper;
using Cart.Domain.Entities;
using Cart.Infrastructure.Cache.Key;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Security.Infrastructure.Cache.Key;
using CartEntity = Cart.Domain.Entities.Cart;

namespace Cart.Infrastructure.Seeders
{
    public class CartSeeder : Seeder, IDevelopmentSeeder
    {
        public IEnumerable<CartEntity> Carts { get; set; }

        public CartSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
            _dependencies.Add(CacheKeyUser.SeedCreatedIdAdmin);
            _dependencies.Add(CacheKeyUser.SeedCreatedIdsClient);
        }
        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(
                CacheKeyCart.SeedIds,
                CacheKeyCart.SeedUserIds,
                CacheKeyCart.SeedCreatedIds,
                CacheKeyCart.SeedCreatedIdsUser
            );
            if (await Set<CartEntity>().AnyAsync(cancellationToken))
            {
                await _cache.SaveAsync(CacheKeyCart.SeedIds, 
                    await Set<CartItem>().OrderByDescending(x => x.CreatedAt).Take(_quantity)
                    .Select(x => x.Id).ToListAsync());
                await _cache.SaveAsync(CacheKeyCart.SeedUserIds,
                    await Set<CartItem>().OrderByDescending(x => x.CreatedAt).Take(_quantity)
                    .Select(x => x.CreatedById).ToListAsync());
                await Task.WhenAll(
                    _cache.SaveAsync(CacheKeyCart.SeedCreatedIds, true),
                    _cache.SaveAsync(CacheKeyCart.SeedCreatedIdsUser, true)
                );
                return Array.Empty<object>();
            }
            await _cache.WaitAsync(_dependencies);
            var userIds = await _cache.GetAsync<List<Guid>>(CacheKeyUser.SeedIdsClient);
            var adminId = await _cache.GetAsync<Guid>(CacheKeyUser.SeedIdAdmin, cancellationToken);
            Carts = Enumerable.Range(1, userIds.Count).Select(i =>
            {
                if(i % 3 == 0)
                {
                    var entity = new CartEntity
                    {
                        Id = Guid.NewGuid()
                    };
                    AddAuditableProperties(entity, userIds.ElementAt(i-1), adminId);
                    return entity;
                }
                return null;
            }).Where(x => x != null).ToList();

            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyCart.SeedIds, Carts.Select(x => x.Id)),
                _cache.SaveAsync(CacheKeyCart.SeedUserIds, Carts.Select(x => x.CreatedById))
            );

            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyCart.SeedCreatedIds, true),
                _cache.SaveAsync(CacheKeyCart.SeedCreatedIdsUser, true)
            );

            return Carts;
        }
    }
}
