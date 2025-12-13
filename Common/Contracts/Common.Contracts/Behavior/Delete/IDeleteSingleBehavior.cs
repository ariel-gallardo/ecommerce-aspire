using Common.Contracts.Behavior.Base;
using Common.Contracts.Entities;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Delete
{
    public interface IDeleteSingleBehavior<DomainEntity,Key> : IServiceBehavior<Key> where DomainEntity : class, IEntity
    {
        Task OnBeforeAsync(Key key, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
