using Common.Domain.Contracts.Repositories;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Contracts;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Security.Domain.Entities;
using Security.Domain.Enums;
using Security.Infrastructure.Cache.Key;
using Security.Infrastructure.Contracts;

namespace Security.Infrastructure.Seeders
{
    public class UserSeeder : Seeder, IDevelopmentSeeder
    {
        private readonly IAuthServices _authServices;
        private IEnumerable<User> Users { get; set; }

        public UserSeeder(IAuthServices authServices, IOptions<AppSettings> options, ICacheManagerServices cache, IUnitOfWork unitOfWork) : base(options, cache, unitOfWork)
        {
            _authServices = authServices;
            Users = Array.Empty<User>();
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
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
            Users = Enumerable.Range(1, _quantity).Select(x =>
            {
                return new User
                {
                    Id = Guid.NewGuid(),
                    Rol = (x % 3 == 0 ? RoleEnum.Administrator : x % 5 == 0 ? RoleEnum.Operator : RoleEnum.Client),
                    Email = $"user_email_{x}@mail.com",
                    Username = $"user_name_{x}",
                    Password = _authServices.HashPassword("123456aA$")
                };
            });
            var userAdmin = Users.First(x => x.Rol == RoleEnum.Administrator);

            await Task.WhenAll(
                _cache.SaveAsync(CacheKeyUser.SeedIdAdmin, userAdmin.Id),
                _cache.SaveAsync(CacheKeyUser.SeedIdsAdmin, Users.Where(x => x.Rol == RoleEnum.Administrator).Select(x => x.Id)),
                _cache.SaveAsync(CacheKeyUser.SeedIdsClient, Users.Where(x => x.Rol == RoleEnum.Client).Select(x => x.Id))
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

            await Task.WhenAll(
                _unitOfWork.Context.AddRangeAsync(Users),
                _cache.SaveAsync(CacheKeyUser.SeedIds, Users)
            );

            await _cache.SaveAsync(CacheKeyUser.SeedCreatedIds, true);
        }
    }
}
