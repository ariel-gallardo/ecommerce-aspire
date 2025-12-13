using Common.Contracts.Behavior.Search.Base;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Search
{
    public interface ISearchQuerieBulkBehavior<DomainEntity, ResultDTO> : ISearchBulkBehavior<ResultDTO>
        where DomainEntity : class, IEntity
        where ResultDTO : class, DTO.Base.IEntityDTO
    {
        Task<IQuerieFilter> OnBeforeAsync(IQuerieFilter filter, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
