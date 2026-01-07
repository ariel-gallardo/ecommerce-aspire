using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Common.Infrastructure.Contracts
{ 
    public interface IEntity
    {
        ILazyLoader LazyLoader { get; }
    }
}
