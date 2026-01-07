namespace Common.Infrastructure.Contracts
{
    public interface IDeleteBulkBehavior<DomainEntity,Key> : IServiceBehavior<Key> where DomainEntity : class, IEntity
    {
        Task OnBeforeAsync(IList<Key> key, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
