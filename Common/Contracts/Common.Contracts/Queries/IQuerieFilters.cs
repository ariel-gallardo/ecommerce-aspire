using System.Linq.Expressions;
using Common.Contracts.Entities;

namespace Common.Contracts.Queries
{
    public interface IQuerieFilters<T> where T : class, IEntity
    {
        Expression<Func<T, bool>> Expressions { get; }
    }
}
