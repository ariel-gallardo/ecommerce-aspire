using Common.Contracts.Behavior.Search.Base;
using Common.Contracts.Entities;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Search
{
    public interface ISearchKeyBulkBehavior<Key, DomainEntity, ResultDTO> : ISearchBulkBehavior<ResultDTO>
        where DomainEntity : class, IEntity
        where ResultDTO : class, DTO.Base.IEntityDTO
    {
        Task OnBeforeAsync(IList<Key> key, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
