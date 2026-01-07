

using Common.Domain.Entities;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Entities.Const;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Infrastructure.Cache.Key;
using Security.Infrastructure.Contracts;
using Security.Infrastructure.Entities;

namespace Security.Infrastructure.Seeders
{
    public class UserSeeder : Seeder, IDevelopmentSeeder
    {
        private readonly IAuthServices _authServices;
        private IEnumerable<User> Users { get; set; }

        public UserSeeder(IAuthServices authServices, IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper ,sp)
        {
            _authServices = authServices;
            Users = Array.Empty<User>();
        }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.RemoveAsync(
                CacheKeyUser.SeedIdAdmin,
                CacheKeyUser.SeedIdsAdmin,
                CacheKeyUser.SeedIdsClient,
                CacheKeyUser.SeedCreatedIdAdmin,
                CacheKeyUser.SeedCreatedIdsAdmin,
                CacheKeyUser.SeedCreatedIdsClient,
                CacheKeyUser.SeedIds,
                CacheKeyUser.SeedCreatedIds
            );
            if (await Set<User>().AnyAsync(cancellationToken))
            {
                
                await Task.WhenAll(
                    _cache.SaveAsync(CacheKeyUser.SeedIdAdmin,await Set<User>().Where(x => x.Rol == Role.Administrator).Select(x => x.Id).FirstAsync()),
                    _cache.SaveAsync(CacheKeyUser.SeedIdsAdmin, await Set<User>().Where(x => x.Rol == Role.Administrator).Select(x => x.Id).Take(_quantity).ToListAsync()),
                    _cache.SaveAsync(CacheKeyUser.SeedIdsClient, await Set<User>().Where(x => x.Rol == Role.Client).Select(x => x.Id).Take(_quantity).ToListAsync()),
                    _cache.SaveAsync(CacheKeyUser.SeedIds, await Set<User>().Select(x => x.Id).Take(_quantity).ToListAsync())
                );
                await Task.WhenAll(
                    _cache.SaveAsync(CacheKeyUser.SeedCreatedIdAdmin,true),
                    _cache.SaveAsync(CacheKeyUser.SeedCreatedIdsAdmin,true),
                    _cache.SaveAsync(CacheKeyUser.SeedCreatedIdsClient,true),
                    _cache.SaveAsync(CacheKeyUser.SeedCreatedIds,true)
                );
                return Array.Empty<object>();
            }
            
            Users = Enumerable.Range(1, _quantity).Select(x =>
            {
                return new User
                {
                    Id = x == 1 ? SecurityConst.InternalAdminId : ulong.Parse(x.ToString()),
                    Rol = x == 1 ? Role.Administrator : (x % 9 == 0 ? Role.Administrator : x % 3 == 0 ? Role.Support : Role.Client),
                    Email = $"user_email_{x}@mail.com",
                    Username = $"user_name_{x}",
                    Password = _authServices.HashPassword("123456aA$")
                };
            });
            var userAdmin = Users.First(x => x.Rol == Role.Administrator);

 
            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyUser.SeedIdAdmin, userAdmin.Id),
                _cache.SaveAsync(CacheKeyUser.SeedIdsAdmin, Users.Where(x => x.Rol == Role.Administrator).Select(x => x.Id)),
                _cache.SaveAsync(CacheKeyUser.SeedIdsClient, Users.Where(x => x.Rol == Role.Client).Select(x => x.Id))
            );
            
            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyUser.SeedCreatedIdAdmin, true),
                _cache.SaveAsync(CacheKeyUser.SeedCreatedIdsAdmin, true),
                _cache.SaveAsync(CacheKeyUser.SeedCreatedIdsClient, true)
            );

            await _cache.WaitAsync(CacheKeyPersona.SeedCreatedIds, cancellationToken);

            var people = await _cache.GetAsync<List<Guid>>(CacheKeyPersona.SeedIds);

            for (int i = 0; i < _quantity-1; i++)
                Users.Where(x => x != userAdmin).ElementAt(i).PersonaId = i % 4 == 0 ? people.ElementAt(i) : null;
            await _cache.SaveAsync(CacheKeyUser.SeedIds, Users);
            await _cache.SaveAsync(CacheKeyUser.SeedCreatedIds, true);
            return Users;
        }
    }
}
