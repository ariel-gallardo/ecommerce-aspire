using Common.Contracts.Behavior.Base;
using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Infrastructure.Entities;

namespace Common.Contracts.Behavior.Update
{
    public interface IUpdateBulkBehavior<UpdateDTO, DomainEntity, ResultDTO> : IServiceBehavior<UpdateDTO, ResultDTO>
        where DomainEntity : class, IEntity
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where ResultDTO : class, IEntityDTO
    {
        Task<IList<UpdateDTO>> OnBeforeAsync(IList<UpdateDTO> entities, CancellationToken cancellationToken);
        Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken);
    }
}
