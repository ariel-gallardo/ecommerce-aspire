namespace Common.Infrastructure.Contracts
{
    public interface IDeleteSingleBehavior<DomainEntity,Key> : IServiceBehavior<Key> where DomainEntity : class, IEntity
    {
        Task OnBeforeAsync(Key key, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
