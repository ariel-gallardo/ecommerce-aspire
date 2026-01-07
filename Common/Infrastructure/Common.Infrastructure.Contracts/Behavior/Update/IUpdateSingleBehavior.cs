namespace Common.Infrastructure.Contracts
{
    public interface IUpdateSingleBehavior<UpdateDTO, DomainEntity, ResultDTO> : IServiceBehavior<UpdateDTO, ResultDTO>
        where DomainEntity : class, IEntity
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where ResultDTO : class, IEntityDTO
    {
        Task<UpdateDTO> OnBeforeAsync(UpdateDTO updateDTO, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
