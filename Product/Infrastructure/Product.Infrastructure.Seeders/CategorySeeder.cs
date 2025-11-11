using Common.Domain.Contracts.Repositories;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Common.Infrastructure.Persistence.Seeds.Base;
using Common.Infrastructure.Seeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Domain.Entities;
using Product.Infrastructure.Messaging.Key;
using Security.Infrastructure.Cache.Key;
using System.Text.Json;

namespace Product.Infrastructure.Seeders
{
    public class CategorySeeder : Seeder, IDevelopmentSeeder
    {
        private IEnumerable<Category> Categories { get; set; }
        public CategorySeeder(IOptions<AppSettings> options, ICacheManagerServices cache, IUnitOfWork unitOfWork) : base(options, cache, unitOfWork)
        {

        }
        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            _cache.SetCancellationToken(cancellationToken);
            await _cache.WaitAsync(CacheKeyUser.SeedCreatedIdsAdmin, cancellationToken);
            var userAdminIds = await _cache.GetAsync<List<Guid>>(CacheKeyUser.SeedIdsAdmin, cancellationToken);
            int total = _quantity;
            int part = total / 3;
            int remainder = total % 3;

            int aCount = part;
            int bCount = part;
            int cCount = part + remainder;

            var catA = Enumerable.Range(1, aCount).Select(i =>
            {
                var entity = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = $"Category {i}",
                    Description = i % 3 == 0 ? $"Description {i}" : null
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();

            var catB = Enumerable.Range(aCount + 1, bCount).Select(i =>
            {
                var entity = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = $"Category {i}",
                    Description = i % 3 == 0 ? $"Description {i}" : null,
                    ParentId = catA[_random.Next(catA.Count)].Id
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();

            var catC = Enumerable.Range(aCount + bCount + 1, cCount).Select(i =>
            {
                var entity = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = $"Category {i}",
                    Description = i % 3 == 0 ? $"Description {i}" : null,
                    ParentId = catB[_random.Next(catB.Count)].Id
                };
                AddAuditableProperties(entity, userAdminIds);
                return entity;
            }).ToList();

            Categories = catA.Concat(catB).Concat(catC);
            await Task.WhenAll(
                _unitOfWork.Context.AddRangeAsync(Categories, cancellationToken),
                _cache.SaveAsync(CacheKeyCategory.SeedIds, catC.Select(x => x.Id))
            );
            await _cache.SaveAsync(CacheKeyCategory.SeedCreatedIds, true);
        }
    }
}
