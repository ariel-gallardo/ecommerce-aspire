namespace Common.Infrastructure.Contracts
{
    public interface ISearchKeySingleBehavior<Key, DomainEntity, ResultDTO> : ISearchSingleBehavior<Key, ResultDTO>
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO
    {
        Task OnBeforeAsync(Key key, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
