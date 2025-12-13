using Common.Contracts.Behavior.Base;
using Common.Contracts.Queries;

namespace Common.Contracts.Behavior.Search.Base
{
    public interface ISearchBulkBehavior<ResultDTO> : IServiceBehavior<IQuerieFilter, ResultDTO> where ResultDTO : class, DTO.Base.IEntityDTO
    {

    }
}
