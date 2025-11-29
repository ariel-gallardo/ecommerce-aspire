using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Common.Contracts
{
    public interface ICommonController<DomainEntity,AddDTO,UpdateDTO,ResultDTO, QuerieFilterEntity> : IController
        where DomainEntity : class, IEntity
        where AddDTO : class, IEntityDTO, IAddDTO
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where ResultDTO : class, IEntityDTO, IResultDTO
        where QuerieFilterEntity : class, IQuerieFilter
    {
        #region Add
        
        Task<IActionResult> AddAsync([FromBody] AddDTO entity, CancellationToken cancellationToken);
        
        Task<IActionResult> AddAsync(IList<AddDTO> entities, CancellationToken cancellationToken);
        #endregion

        #region Update
        
        Task<IActionResult> UpdateAsync(UpdateDTO entity, CancellationToken cancellationToken);
        
        Task<IActionResult> UpdateAsync(IList<UpdateDTO> entities, CancellationToken cancellationToken);
        #endregion

        #region Delete
        
        Task<IActionResult> DeleteAsync(ulong entityId, CancellationToken cancellationToken);
        
        Task<IActionResult> DeleteAsync(IList<ulong> entityIds, CancellationToken cancellationToken);
        #endregion

        #region Search
        
        Task<IActionResult> SearchAsync(ulong entityId, CancellationToken cancellationToken);

        Task<IActionResult> SearchFirstAsync([FromQuery] QuerieFilterEntity filters, CancellationToken cancellationToken);
        Task<IActionResult> SearchAsync(QuerieFilterEntity filters, CancellationToken cancellationToken);
        
        Task<IActionResult> SearchAsync(IList<ulong> entityIds, int page, int pageSize, CancellationToken cancellationToken);
        #endregion
    }
}
