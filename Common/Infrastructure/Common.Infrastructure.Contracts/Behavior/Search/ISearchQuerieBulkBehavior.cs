namespace Common.Infrastructure.Contracts
{
    public interface ISearchQuerieBulkBehavior<DomainEntity, ResultDTO> : ISearchBulkBehavior<ResultDTO>
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO
    {
        Task<IQuerieFilter> OnBeforeAsync(IQuerieFilter filter, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
