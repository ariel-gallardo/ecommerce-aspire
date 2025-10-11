using System.Linq.Expressions;

namespace Common.Domain.Entities.Contracts
{
    public interface IQuerieFilters<T> where T : class, IEntity
    {
        Expression<Func<T, bool>> Expressions { get; }
    }
}
