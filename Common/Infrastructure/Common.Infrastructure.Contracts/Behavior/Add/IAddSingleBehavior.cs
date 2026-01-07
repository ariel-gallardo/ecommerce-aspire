namespace Common.Infrastructure.Contracts
{
    public interface IAddSingleBehavior<AddDTO, ResultDTO> : IServiceBehavior<AddDTO, ResultDTO>
    where AddDTO : class, IEntityDTO, IAddDTO
    where ResultDTO : class, IEntityDTO, IReadDTO
    {
        Task<AddDTO> OnBeforeAsync(AddDTO addDTO, CancellationToken cancellationToken);
        Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken);
    }
}
