namespace Common.Infrastructure.Contracts
{
    public interface IUpdateBulkBehavior<UpdateDTO, DomainEntity, ResultDTO> : IServiceBehavior<UpdateDTO, ResultDTO>
        where DomainEntity : class, IEntity
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where ResultDTO : class, IEntityDTO
    {
        Task<IList<UpdateDTO>> OnBeforeAsync(IList<UpdateDTO> entities, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
