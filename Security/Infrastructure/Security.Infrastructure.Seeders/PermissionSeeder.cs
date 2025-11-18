using AutoMapper;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Entities.Const;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Domain.Entities;
using Security.Infrastructure.Cache.Key;

namespace Security.Infrastructure.Seeders
{
    public class PermissionSeeder : Seeder, IDevelopmentSeeder
    {
        public PermissionSeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IMapper mapper, IServiceProvider sp) : base(options, cache, mapper, sp)
        {
            _dependencies.Add(CacheKeyUser.SeedCreatedIdAdmin);
        }
        public IEnumerable<Permission> Permissions { get; set; }

        public async Task<IEnumerable<object>> SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await Set<Permission>().AnyAsync(cancellationToken))
            {
                return Array.Empty<Permission>();
            }
            _cache.SetCancellationToken(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await _cache.WaitAsync(_dependencies);
            var adminId = await _cache.GetAsync<long>(CacheKeyUser.SeedIdAdmin);
            var publicRoutes = new Permission[]
            {
                new Permission{ Controller = "Users", Action = "Login", Policy = Policy.Public, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "Register", Policy = Policy.Public, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/login", Policy = Policy.Public, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/register", Policy = Policy.Public, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            };
            Permissions = Enumerable.Range(1, _quantity).Select(i => new Permission
            {
                Url = i % 2 != 0 ? $"/path_{_random.Next(1, 100)}/sub_path_{_random.Next(1, 100)}" : string.Empty,
                Action = i % 2 == 0 ? $"Action {_random.Next(1, 30)}" : string.Empty,
                Controller = i % 2 == 0 ? $"Controller {_random.Next(1, 30)}" : string.Empty,
                Policy = i % 3 == 0 ? (i % 7 == 0 ? Policy.Administrator : (i % 4 == 0 ? Policy.Client : Policy.Public)) : Policy.Unknown,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).Concat(publicRoutes);
            return Permissions;
        }
    }
}
