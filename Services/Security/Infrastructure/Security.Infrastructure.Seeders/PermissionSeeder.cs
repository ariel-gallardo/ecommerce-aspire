
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
                new Permission{ Controller = "Users", Action = "Add", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "Update", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "Delete", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "Search", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Users", Action = "SearchFirst", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion
                
                #region Personas
                new Permission{ Controller = "Personas", Action = "Add", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "Update", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "Delete", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "Search", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Personas", Action = "SearchFirst", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                #endregion

                #region Permission
                new Permission{ Controller = "Permission", Action = "Add", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "Update", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "Delete", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "Search", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "SearchFirst", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Permission", Action = "CanAccess", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                #endregion
                
                #region Error
                new Permission{ Controller = "Error", Action = "Add", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "Update", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "Delete", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "Search", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Error", Action = "SearchFirst", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion

                #region Info
                new Permission{ Controller = "Info", Action = "Add", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "Update", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "Delete", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "Search", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                new Permission{ Controller = "Info", Action = "SearchFirst", CreatedById = adminId, Policy = Policy.Support, CreatedAt = DateTime.UtcNow },
                #endregion

                #region Urls
                new Permission{ Url="/users/login", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/register", CreatedById = adminId, Policy = Policy.Public, CreatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/profile", CreatedById = adminId, Policy = Policy.Client, CreatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/admin", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/admin/logs", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                new Permission{ Url="/users/admin/permissions", CreatedById = adminId, Policy = Policy.Administrator, CreatedAt = DateTime.UtcNow },
                #endregion
            };
            
            Permissions = publicRoutes;
            return Permissions;
        }
    }
}
