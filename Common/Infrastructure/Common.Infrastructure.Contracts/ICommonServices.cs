using Microsoft.AspNetCore.Mvc;

namespace Common.Infrastructure.Contracts
{
    public interface ICommonServices
    {
        #region Add
        Task<IResponse> AddAsync<AddDTO,DomainEntity,ResultDTO>([FromBody] AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;

        Task<IResponse> AddAsync<AddDTO, DomainEntity, ResultDTO>([FromBody] IList<AddDTO> entities, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;
        #endregion

        #region Update
        Task<IResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>([FromBody] UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;
        Task<IResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>([FromBody] IList<UpdateDTO> entities, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;
        #endregion

        #region Delete
        Task<IResponse> DeleteAsync<Key,DomainEntity>([FromQuery] Key entityId, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity;
        Task<IResponse> DeleteAsync<Key,DomainEntity>([FromBody] IList<Key> entityIds, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity;
        #endregion

        #region Search
        Task<IResponse> SearchAsync<Key,DomainEntity, ResultDTO>([FromQuery] Key entityId, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;

        Task<IResponse> SearchFirstAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO;
        Task<IResponse> SearchAsync<DomainEntity, ResultDTO>([FromBody] IQuerieFilter filters, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;
        Task<IResponse> SearchAsync<Key,DomainEntity, ResultDTO>([FromBody] IList<Key> entityIds, int page, int pageSize, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IReadDTO;
        #endregion
    }
}
