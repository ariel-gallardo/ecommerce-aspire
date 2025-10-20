using Common.Contracts;
using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Common.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Common.Domain.Contracts.Repositories
{
    public interface IUnitOfWork : IScoped
    {
        #region Add
        Task<DomainEntity> AddAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken) 
            where DomainEntity : class, IEntity;
        Task<ResultDTO> AddAsync<AddDTO, DomainEntity, ResultDTO>(AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IResultDTO, IEntityDTO;
        Task<IList<DomainEntity>> AddAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        Task<IList<ResultDTO>> AddAsync<AddDTO, DomainEntity, ResultDTO>(IList<AddDTO> entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IResultDTO, IEntityDTO;
        #endregion

        #region Update
        Task<DomainEntity> UpdateAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken) 
            where DomainEntity : class, IEntity;
        Task<ResultDTO> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IUpdateDTO, IEntityDTO 
            where DomainEntity : class, IEntity
            where ResultDTO : class, IResultDTO, IEntityDTO;
        Task<IList<DomainEntity>> UpdateAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        Task<IList<ResultDTO>> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(IList<UpdateDTO> entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IUpdateDTO, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IResultDTO, IEntityDTO;
        #endregion


        #region Delete
        Task DeleteAsync<DomainEntity>(Guid id, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        Task DeleteAsync<DomainEntity>(IList<Guid> id, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        #endregion

        #region Exists
        Task<bool> ExistsAsync<DomainEntity>(Guid id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<(bool, IList<Guid>)> ExistsAsync<DomainEntity>(IList<Guid> ids, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<bool> ExistsAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        #endregion

        #region SearchOne
        Task<DomainEntity> SearchOneAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<ResultDTO> SearchOneAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion

        #region Search
        Task<DomainEntity> SearchAsync<DomainEntity>(Guid id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<ResultDTO> SearchAsync<DomainEntity, ResultDTO>(Guid id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO;

        Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(IList<Guid> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(IList<Guid> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO;
        Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO;

        Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO;
        #endregion

        #region Transactions
        Task BeginTransaction(CancellationToken cancellationToken);
        Task CreateSavePoint(string name, CancellationToken cancellationToken);
        Task RollbackToSavepoint(string name, CancellationToken cancellationToken);
        Task ReleaseSavepoint(string name, CancellationToken cancellationToken);
        Task CommitTransaction(CancellationToken cancellationToken);
        Task RollbackTransaction(CancellationToken cancellationToken);
        #endregion
    }
}
