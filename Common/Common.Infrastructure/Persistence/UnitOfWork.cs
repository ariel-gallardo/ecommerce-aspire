using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Domain.DTOS.Base.Contracts;
using Common.Domain.DTOS.Base.Entities;
using Common.Domain.Entities.Base;
using Common.Domain.Entities.Contracts;
using Common.Domain.Enums;
using Common.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Common.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _ctx;
        private readonly IMapper _map;
        private readonly IAuthServices _usrServices;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DbContext context, IMapper mapper, IAuthServices userServices)
        {
            _ctx = context;
            _map = mapper;
            _usrServices = userServices;
        }

        #region ABM
        public async Task AddAsync<OnDB>(OnDB entity, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
        {
            if(entity is IAuditable a)
            {
                a.CreatedAt = DateTime.UtcNow;
                a.CreatedById = _usrServices.Id;
                _ctx.Entry(a).Property(x => x.CreatedBy).IsModified = false;
                _ctx.Entry(a).Property(x => x.UpdatedAt).IsModified = false;
                _ctx.Entry(a).Property(x => x.UpdatedBy).IsModified = false;
                _ctx.Entry(a).Property(x => x.UpdatedById).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedAt).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedBy).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedById).IsModified = false;
                await _ctx.AddAsync(a, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
                return;
            }
            await _ctx.AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync<OnDTO,OnDB>(OnDTO entity, CancellationToken cancellationToken)
            where OnDTO : class, IIdentifiableDTO
            where OnDB : class, IIdentifiable
            => await AddAsync(_map.Map<OnDB>(entity), cancellationToken);

        public async Task UpdateAsync<OnDB>(OnDB entity, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
        {
            if (entity is IIdentifiable iE)
            {
                if (!await ExistsById<OnDB>(iE.Id, cancellationToken))
                    throw new EntityNotFoundException(typeof(OnDB).Name, ActionEnum.Update, iE.Id);
            }
            if (entity is IAuditable a)
            {
                a.UpdatedById = _usrServices.Id;
                a.UpdatedAt = DateTime.UtcNow;
                _ctx.Entry(a).Property(x => x.CreatedAt).IsModified = false;
                _ctx.Entry(a).Property(x => x.CreatedBy).IsModified = false;
                _ctx.Entry(a).Property(x => x.CreatedById).IsModified = false;
                _ctx.Entry(a).Property(x => x.UpdatedBy).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedAt).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedBy).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedById).IsModified = false;
                _ctx.Update(a);
                await _ctx.SaveChangesAsync(cancellationToken);
                return;
            }
            _ctx.Update(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync<OnDTO, OnDB>(OnDTO entity, CancellationToken cancellationToken)
            where OnDTO : class, IIdentifiableDTO
            where OnDB : class, IIdentifiable
            => await UpdateAsync(_map.Map<OnDB>(entity), cancellationToken);

        public async Task DeleteAsync<OnDB>(Guid id, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiableDTO
        {
            try
            {
                var entity = await _ctx.Set<OnDB>().FirstAsync(x => Guid.Parse(x.Id) == id, cancellationToken);
                if (entity is IAuditable a)
                {
                    a.DeletedById = _usrServices.Id;
                    a.DeletedAt = DateTime.UtcNow;
                    _ctx.Entry(a).Property(x => x.CreatedBy).IsModified = false;
                    _ctx.Entry(a).Property(x => x.CreatedById).IsModified = false;
                    _ctx.Entry(a).Property(x => x.CreatedAt).IsModified = false;
                    _ctx.Entry(a).Property(x => x.UpdatedAt).IsModified = false;
                    _ctx.Entry(a).Property(x => x.UpdatedBy).IsModified = false;
                    _ctx.Entry(a).Property(x => x.UpdatedById).IsModified = false;
                    _ctx.Entry(a).Property(x => x.DeletedBy).IsModified = false;
                    _ctx.Update(entity);
                    await _ctx.SaveChangesAsync(cancellationToken);
                    return;
                }
            }
            catch (Exception ex) 
            {
                throw new EntityNotFoundException(typeof(OnDB).Name, ActionEnum.Delete, id, ex);
            }
        }
        #endregion

        #region FindById
        public async Task<bool> ExistsById<OnDB>(Guid id, CancellationToken cancellationToken)
        where OnDB : class, IIdentifiable
        => await _ctx.Set<OnDB>().AsNoTracking().Where(x => x.Id == id).AnyAsync(cancellationToken);

        public async Task<OnDB> FirstOrDefaultByIdAsync<OnDB>(Guid id, CancellationToken cancellationToken)
        where OnDB : class, IIdentifiable
            => await _ctx.Set<OnDB>().AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task<OnDTO> FirstOrDefaultByIdAsync<OnDB, OnDTO>(Guid id, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
            where OnDTO : class
            => await _ctx.Set<OnDB>().AsNoTracking().Where(x => x.Id == id).ProjectTo<OnDTO>(_map.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        #endregion

        #region Expressions
        public async Task<List<OnDB>> GetAllAsync<OnDB>(Expression<Func<OnDB, bool>> where, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
             => await _ctx.Set<OnDB>().AsNoTracking().Where(where).ToListAsync(cancellationToken);

        public async Task<List<OnDTO>> GetAllAsync<OnDB,OnDTO>(Expression<Func<OnDB, bool>> where, CancellationToken cancellationToken)
            where OnDB : class, IIdentifiable
            where OnDTO : class, IIdentifiableDTO
             => await _ctx.Set<OnDB>().AsNoTracking().Where(where).ProjectTo<OnDTO>(_map.ConfigurationProvider).ToListAsync(cancellationToken);
        #endregion

        #region QuerieFilters
        public async Task<OnDB> FirstOrDefaultByQuerieFiltersAsync<OnDB>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IIdentifiable
        => await _ctx.Set<OnDB>().Where(filters.Expressions).FirstOrDefaultAsync(cancellationToken);

        public async Task<OnDTO> FirstOrDefaultByQuerieFiltersAsync<OnDB, OnDTO>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IIdentifiable
        where OnDTO : class
        => await _ctx.Set<OnDB>().Where(filters.Expressions).ProjectTo<OnDTO>(_map.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        public async Task<List<OnDB>> GetAllByQuerieFiltersAsync<OnDB>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IIdentifiable
        => await _ctx.Set<OnDB>().Where(filters.Expressions).ToListAsync(cancellationToken);

        public async Task<List<OnDTO>> GetAllQuerieFiltersAsync<OnDB, OnDTO>(IQuerieFilters<OnDB> filters, CancellationToken cancellationToken)
        where OnDB : class, IIdentifiable
        where OnDTO : class
        => await _ctx.Set<OnDB>().Where(filters.Expressions).ProjectTo<OnDTO>(_map.ConfigurationProvider).ToListAsync(cancellationToken);
        #endregion

        #region Transaction
        public async Task BeginTransaction(CancellationToken cancellationToken)
        {
            _transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CreateSavePoint(string name, CancellationToken cancellationToken)
        => await _transaction.CreateSavepointAsync(name, cancellationToken);

        public async Task RollbackToSavepoint(string name, CancellationToken cancellationToken)
        => await _transaction.RollbackToSavepointAsync(name, cancellationToken);

        public async Task ReleaseSavepoint(string name, CancellationToken cancellationToken)
        => await _transaction.ReleaseSavepointAsync(name, cancellationToken);

        public async Task CommitTransaction(CancellationToken cancellationToken)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransaction(CancellationToken cancellationToken)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        #endregion
    }
}
