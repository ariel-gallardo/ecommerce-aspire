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
        Task<Response> AddAsync<AddDTO,DomainEntity,ResultDTO>([FromBody] AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;

        Task<Response> AddAsync<AddDTO, DomainEntity, ResultDTO>([FromBody] IList<AddDTO> entities, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion

        #region Update
        Task<Response> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>([FromBody] UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<Response> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>([FromBody] IList<UpdateDTO> entities, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion

        #region Delete
        Task<Response> DeleteAsync<DomainEntity>([FromQuery] Guid entityId, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        Task<Response> DeleteAsync<DomainEntity>([FromBody] IList<Guid> entityIds, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        #endregion

        #region Search
        Task<Response> SearchAsync<DomainEntity, ResultDTO>([FromQuery] Guid entityId, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<Response> SearchAsync<DomainEntity, ResultDTO>([FromBody] IQuerieFilter filters, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<Response> SearchAsync<DomainEntity, ResultDTO>([FromBody] IList<Guid> entityIds, int page, int pageSize, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion
    }
}
