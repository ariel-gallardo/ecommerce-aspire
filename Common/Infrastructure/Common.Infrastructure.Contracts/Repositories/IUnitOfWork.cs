using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Infrastructure.Contracts
{
    public interface IUnitOfWork : IScoped
    {
        DbContext Context { get; }
        #region Add
        Task<DomainEntity> AddAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken) 
            where DomainEntity : class, IEntity;
        Task<ResultDTO> AddAsync<AddDTO, DomainEntity, ResultDTO>(AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class;
        Task<IList<DomainEntity>> AddAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        Task<IList<ResultDTO>> AddAsync<AddDTO, DomainEntity, ResultDTO>(IList<AddDTO> entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class;
        #endregion

        #region Update
        Task<DomainEntity> UpdateAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken) 
            where DomainEntity : class, IEntity;
        Task<ResultDTO> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IUpdateDTO, IEntityDTO 
            where DomainEntity : class, IEntity
            where ResultDTO : class;
        Task<IList<DomainEntity>> UpdateAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity;
        Task<IList<ResultDTO>> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(IList<UpdateDTO> entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IUpdateDTO, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class;
        #endregion


        #region Delete
        Task DeleteAsync<Key,DomainEntity>(Key id, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity;
        Task DeleteAsync<Key,DomainEntity>(IList<Key> id, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity;
        #endregion

        #region Exists
        Task<bool> ExistsAsync<Key,DomainEntity>(Key id, CancellationToken cancellationToken)
        
        where DomainEntity : class, IEntity;
        Task<(bool, IList<Key>)> ExistsAsync<Key,DomainEntity>(IList<Key> ids, CancellationToken cancellationToken)
        
        where DomainEntity : class, IEntity;
        Task<bool> ExistsAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        #endregion

        #region SearchOne
        Task<DomainEntity> SearchOneAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<ResultDTO> SearchOneAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class;
        #endregion

        #region Search
        Task<DomainEntity> SearchAsync<Key, DomainEntity>(Key id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<ResultDTO> SearchAsync<Key, DomainEntity, ResultDTO>(Key id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class;

        Task<IPagedList<DomainEntity>> SearchAsync<Key,DomainEntity>(IList<Key> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<IPagedList<ResultDTO>> SearchAsync<Key,DomainEntity, ResultDTO>(IList<Key> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class;
        Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class;

        Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity;
        Task<ResultDTO> SearchFirstAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class;
        Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class;
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
