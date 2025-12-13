using Common.Contracts.Behavior.Search.Base;
using Common.Contracts.Entities;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Search
{
    public interface ISearchKeySingleBehavior<Key, DomainEntity, ResultDTO> : ISearchSingleBehavior<Key, ResultDTO>
        where DomainEntity : class, IEntity
        where ResultDTO : class, DTO.Base.IEntityDTO
    {
        Task OnBeforeAsync(Key key, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
