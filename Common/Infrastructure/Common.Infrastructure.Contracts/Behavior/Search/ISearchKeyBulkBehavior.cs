namespace Common.Infrastructure.Contracts
{
    public interface ISearchKeyBulkBehavior<Key, DomainEntity, ResultDTO> : ISearchBulkBehavior<ResultDTO>
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO
    {
        Task OnBeforeAsync(IList<Key> key, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
