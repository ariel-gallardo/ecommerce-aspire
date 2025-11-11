
using Common.Domain.Contracts.Entities;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Enums;
using Common.Domain.ValueObjects;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Common.Infrastructure.Seeder.Entities
{
    public abstract class Seeder
    {
        protected readonly Random _random;
        protected readonly AppSettings _appSettings;
        protected readonly ICacheManagerServices _cache;
        protected readonly DistributedCacheEntryOptions _cacheOptions;
        protected readonly int _quantity;
        protected readonly List<string> _dependencies;
        protected readonly IUnitOfWork _unitOfWork;

        protected Seeder(IOptions<AppSettings> options, ICacheManagerServices cache, IUnitOfWork unitOfWork)
        {
            _random = new Random();
            _appSettings = options.Value;
            _cache = cache;
            _cacheOptions = _appSettings.Redis.DistributedCacheEntryOptions;
            _quantity = _appSettings.QuantityToGenerate;
            _dependencies = new List<string>();
            _unitOfWork = unitOfWork;
        }
        protected decimal RandomDecimalByBase(Unit unit)
        {
            var num = _random.Next(1, 10);
            switch (unit)
            {
                case Unit.Gram:
                case Unit.Mililiter:
                    return Decimal.Parse($"{_random.Next(1, 100)}.{_random.Next(0,9)}{_random.Next(0, 9)}");
                default: 
                    return _random.Next(1,10);
            }
        }

        protected Unit RandomUnitBase()
        {
            var num = _random.Next(3);
            switch (num)
            {
                case 1:
                    return Unit.Kilogram;
                case 2:
                    return Unit.Liter;
                default:
                    return Unit.Unit;
            }
        }

        protected Quantity RandomQuantityByUnitBase(Unit unit)
        {
            var num = _random.Next(1, 10);
            switch (unit)
            {
                case Unit.Kilogram:
                    return num % 2 == 0 ? new Quantity
                    {
                        Unit = Unit.Gram,
                        Value = decimal.Parse($"{_random.Next(1,100)}.{_random.Next(0,9)}{_random.Next(0, 9)}")
                    } : new Quantity
                    {
                        Unit = Unit.Kilogram,
                        Value = _random.Next(1, 10)
                    };
                case Unit.Liter:
                    return num % 2 == 0 ? new Quantity
                    {
                        Unit = Unit.Mililiter,
                        Value = decimal.Parse($"{_random.Next(1, 100)}.{_random.Next(0, 9)}{_random.Next(0, 9)}")
                    } : new Quantity
                    {
                        Unit = Unit.Liter,
                        Value = _random.Next(1, 10)
                    };
                default: return new Quantity
                {
                    Unit = Unit.Unit,
                    Value = _random.Next(1, 10)
                };
            }
        }

        protected Unit RandomUnitByBase(Unit unit)
        {
            var num = _random.Next(1, 10);
            switch (unit)
            {
                case Unit.Kilogram:
                    return num % 2 == 0 ? Unit.Gram : Unit.Kilogram;
                case Unit.Liter:
                    return num % 2 == 0 ? Unit.Mililiter : Unit.Liter;
                default: return Unit.Unit;
            }
        }

        protected Price RandomPrice()
        {
            var num = _random.Next(1, 10);
            if (num % 2 == 0)
                return new Price
                {
                    Unit = _random.Next(1, 30) % 6 == 0 ? Unit.USD : Unit.ARS,
                    Value = Decimal.Parse($"{_random.Next(1, 99)}.{_random.Next(0,9)}{_random.Next(0, 9)}")
                };
            return null;
        }

        protected void AddAuditableProperties(IAuditable entity, IList<Guid> userIds)
        {
            var createdByIdNumber = _random.Next(0, userIds.Count() - 1);
            var updatedByIdNumber = _random.Next(0, userIds.Count() - 1);
            var deletedByIdNumber = _random.Next(0, userIds.Count() - 1);
            entity.CreatedById = userIds.ElementAt(createdByIdNumber);
            entity.UpdatedById = updatedByIdNumber % 2 == 0 ? userIds.ElementAt(updatedByIdNumber) : null;
            entity.DeletedById = deletedByIdNumber % 7 == 0 ? userIds.ElementAt(deletedByIdNumber) : null;
            entity.CreatedAt = DateTime.UtcNow;
            if(entity.UpdatedById.HasValue) entity.UpdatedAt = DateTime.UtcNow.AddMinutes(_random.Next(1,30));
            if(entity.DeletedById.HasValue) entity.DeletedAt = DateTime.UtcNow.AddMinutes(_random.Next(30,100));
        }

        protected void AddAuditableProperties(IAuditable entity, Guid userId, Guid adminId)
        {
 
            var updatedByIdNumber = _random.Next(1, 100);
            var deletedByIdNumber = _random.Next(1, 100);
            entity.CreatedById = userId;
            entity.UpdatedById = updatedByIdNumber % 2 == 0 ? userId : null;
            entity.DeletedById = deletedByIdNumber % 7 == 0 ? adminId : null;
            entity.CreatedAt = DateTime.UtcNow;
            if (entity.UpdatedById.HasValue) entity.UpdatedAt = DateTime.UtcNow.AddMinutes(_random.Next(1, 30));
            if (entity.DeletedById.HasValue) entity.DeletedAt = DateTime.UtcNow.AddMinutes(_random.Next(30, 100));
        }
    }
}
