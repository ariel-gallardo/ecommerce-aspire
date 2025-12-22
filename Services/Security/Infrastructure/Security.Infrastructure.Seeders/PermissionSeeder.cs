
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Entities.Const;
using Common.Infrastructure.Entities.Enums;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using MapsterMapper;
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
            var adminId = await _cache.GetAsync<ulong>(CacheKeyUser.SeedIdAdmin);
            var publicRoutes = new Permission[]
            {
                #region Users
                new Permission{ Controller = "Users", Action = "Login", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "Register", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "AddAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "UpdateAsync", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "DeleteAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "SearchAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "SearchFirstAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion
                
                #region Personas
                new Permission{ Controller = "Personas", Action = "AddAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "UpdateAsync", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "DeleteAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "SearchAsync", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "SearchFirstAsync", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                #endregion

                #region Permission
                new Permission{ Controller = "Permission", Action = "AddAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "UpdateAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "DeleteAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "SearchAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "SearchFirstAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion
                
                #region Error
                new Permission{ Controller = "Error", Action = "AddAsync", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "UpdateAsync", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "DeleteAsync", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "SearchAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "SearchFirstAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion

                #region Info
                new Permission{ Controller = "Info", Action = "AddAsync", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "UpdateAsync", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "DeleteAsync", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "SearchAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "SearchFirstAsync", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion

                #region Urls
                new Permission{ Url="/users/login", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/register", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                #endregion
            };
            
            Permissions = publicRoutes;
            return Permissions;
        }
    }
}
