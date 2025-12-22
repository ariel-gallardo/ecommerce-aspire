using Microsoft.AspNetCore.Mvc;

namespace Common.Infrastructure.Contracts
{
    public interface ICommonController<Key, DomainEntity, AddDTO,UpdateDTO,ResultDTO, QuerieFilterEntity> : IController
        
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
        
        Task<IActionResult> DeleteAsync(Key entityId, CancellationToken cancellationToken);
        
        Task<IActionResult> DeleteAsync(IList<Key> entityIds, CancellationToken cancellationToken);
        #endregion

        #region Search
        
        Task<IActionResult> SearchAsync(Key entityId, CancellationToken cancellationToken);

        Task<IActionResult> SearchFirstAsync([FromQuery] QuerieFilterEntity filters, CancellationToken cancellationToken);
        Task<IActionResult> SearchAsync(QuerieFilterEntity filters, CancellationToken cancellationToken);
        
        Task<IActionResult> SearchAsync(IList<Key> entityIds, int page, int pageSize, CancellationToken cancellationToken);
        #endregion
    }
}
