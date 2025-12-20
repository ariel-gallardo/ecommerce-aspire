using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Common.Contracts.Entities
{ 
    public interface IEntity
    {
        ILazyLoader LazyLoader { get; }
    }
}
