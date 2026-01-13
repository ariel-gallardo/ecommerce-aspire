using Microsoft.AspNetCore.Mvc;

namespace Common.Infrastructure.Contracts
{
    public interface ICommonController<Key, DomainEntity, AddDTO,UpdateDTO,ResultDTO, QuerieFilterEntity> : IController
        
        where DomainEntity : class, IEntity
        where AddDTO : class, IEntityDTO, IAddDTO
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where ResultDTO : class, IEntityDTO, IReadDTO
        where QuerieFilterEntity : class, IQuerieFilter
    {
        #region Add
        Task<ActionResult<IResponse<ResultDTO>>> AddAsync([FromBody] AddDTO entity, CancellationToken cancellationToken);
        
        Task<ActionResult<IResponse<IList<ResultDTO>>>> AddAsync(IList<AddDTO> entities, CancellationToken cancellationToken);
        #endregion

        #region Update

        Task<ActionResult<IResponse<ResultDTO>>> UpdateAsync(UpdateDTO entity, CancellationToken cancellationToken);

        Task<ActionResult<IResponse<IList<ResultDTO>>>> UpdateAsync(IList<UpdateDTO> entities, CancellationToken cancellationToken);
        #endregion

        #region Delete
        
        Task<ActionResult<IResponse>> DeleteAsync(Key entityId, CancellationToken cancellationToken);
        
        Task<ActionResult<IResponse>> DeleteAsync(IList<Key> entityIds, CancellationToken cancellationToken);
        #endregion

        #region Search
        
        Task<ActionResult<IResponse<ResultDTO>>> SearchAsync(Key entityId, CancellationToken cancellationToken);

        Task<ActionResult<IResponse<ResultDTO>>> SearchFirstAsync([FromQuery] QuerieFilterEntity filters, CancellationToken cancellationToken);
        Task<ActionResult<IResponse<IPagedList<ResultDTO>>>> SearchAsync(QuerieFilterEntity filters, CancellationToken cancellationToken);
        
        Task<ActionResult<IResponse<IPagedList<ResultDTO>>>> SearchAsync(IList<Key> entityIds, int page, int pageSize, CancellationToken cancellationToken);
        #endregion
    }
}
