namespace Common.Infrastructure.Contracts
{
    public interface IAddBulkBehavior<AddDTO, ResultDTO> : IServiceBehavior<AddDTO, ResultDTO>
    where AddDTO : class, IEntityDTO, IAddDTO
        where ResultDTO : class, IEntityDTO, IResultDTO
    {
        Task<IList<AddDTO>> OnBeforeAsync(IList<AddDTO> addBulkDTO, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
