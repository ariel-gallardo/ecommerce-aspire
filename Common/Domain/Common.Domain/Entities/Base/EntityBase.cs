using Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Common.Domain.Entities.Base
{
    public class EntityBase : IEntity
    {
        protected ILazyLoader _lazyLoader;
        public ILazyLoader LazyLoader { get => _lazyLoader; }
    }
}
