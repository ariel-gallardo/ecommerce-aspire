using Common.Contracts.Behavior.Base;
using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Add
{
    public interface IAddBulkBehavior<AddDTO, ResultDTO> : IServiceBehavior<AddDTO, ResultDTO>
    where AddDTO : class, IEntityDTO, IAddDTO
        where ResultDTO : class, IEntityDTO, IResultDTO
    {
        Task<IList<AddDTO>> OnBeforeAsync(IList<AddDTO> addBulkDTO, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
