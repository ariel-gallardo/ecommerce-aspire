using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Common.Contracts
{
    public interface ICommonServices : IScoped
    {
        #region Add
        Task<BaseResponse> AddAsync<AddDTO,DomainEntity,ResultDTO>([FromBody] AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;

        Task<BaseResponse> AddAsync<AddDTO, DomainEntity, ResultDTO>([FromBody] IList<AddDTO> entities, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion

        #region Update
        Task<BaseResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>([FromBody] UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<BaseResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>([FromBody] IList<UpdateDTO> entities, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion

        #region Delete
        Task<BaseResponse> DeleteAsync<Key,DomainEntity>([FromQuery] Key entityId, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity;
        Task<BaseResponse> DeleteAsync<Key,DomainEntity>([FromBody] IList<Key> entityIds, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity;
        #endregion

        #region Search
        Task<BaseResponse> SearchAsync<Key,DomainEntity, ResultDTO>([FromQuery] Key entityId, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;

        Task<BaseResponse> SearchFirstAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<BaseResponse> SearchAsync<DomainEntity, ResultDTO>([FromBody] IQuerieFilter filters, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<BaseResponse> SearchAsync<Key,DomainEntity, ResultDTO>([FromBody] IList<Key> entityIds, int page, int pageSize, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion
    }
}
