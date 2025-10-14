using Common.Contracts;
using Common.Contracts.DTOS;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using System.Linq.Expressions;

namespace Common.Domain.Contracts.Repositories
{
    public interface IUnitOfWork : IScoped
    {
        #region ABM
        Task<OnDB> AddAsync<OnDB>(OnDB entity, CancellationToken cancellationToken) 
            where OnDB : class, IIdentifiable;
        Task<OnResult> AddAsync<OnDTO, OnDB, OnResult>(OnDTO entity, CancellationToken cancellationToken) 
            where OnDTO : class, IIdentifiableDTO 
            where OnDB : class, IIdentifiable
            where OnResult : class;
        Task<OnDB> UpdateAsync<OnDB>(OnDB entity, CancellationToken cancellationToken) 
            where OnDB : class, IIdentifiable;
        Task<OnResult> UpdateAsync<OnDTO, OnDB, OnResult>(OnDTO entity, CancellationToken cancellationToken)
            where OnDTO : class, IIdentifiableDTO 
            where OnDB : class, IIdentifiable
            where OnResult : class;
        Task DeleteAsync<OnDB>(Guid id, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiableDTO;
        #endregion

        #region FindById
        Task<bool> ExistsById<OnDB>(Guid id, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable;
        Task<OnDB> FirstOrDefaultByIdAsync<OnDB>(Guid id, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable;
        Task<OnDTO> FirstOrDefaultByIdAsync<OnDB, OnDTO>(Guid id, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
            where OnDTO : class;
        #endregion

        #region Expressions
        Task<List<OnDB>> GetAllAsync<OnDB>(Expression<Func<OnDB, bool>> where, CancellationToken cancellationToken)
             where OnDB : class, IIdentifiable;
        Task<List<OnDTO>> GetAllAsync<OnDB,OnDTO>(Expression<Func<OnDB, bool>> where, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
            where OnDTO : class, IIdentifiableDTO;
        #endregion

        #region QuerieFilters
        Task<OnDB> FirstOrDefaultByQuerieFiltersAsync<OnDB>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IEntity;
        Task<OnDTO> FirstOrDefaultByQuerieFiltersAsync<OnDB, OnDTO>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IEntity
        where OnDTO : class;

        Task<bool> ExistsByQuerieFiltersAsync<OnDB>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IEntity;

        Task<List<OnDB>> GetAllByQuerieFiltersAsync<OnDB>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IEntity;
        Task<List<OnDTO>> GetAllQuerieFiltersAsync<OnDB, OnDTO>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IEntity
        where OnDTO : class;
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
