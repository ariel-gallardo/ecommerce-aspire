using Common.Contracts.Behavior.Base;
using Common.Contracts.Entities;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Delete
{
    public interface IDeleteBulkBehavior<DomainEntity,Key> : IServiceBehavior<Key> where DomainEntity : class, IEntity
    {
        Task OnBeforeAsync(IList<Key> key, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
