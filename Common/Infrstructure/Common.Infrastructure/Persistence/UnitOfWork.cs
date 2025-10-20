using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Contracts;
using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Common.Domain.Contracts.Entities;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Domain.Entities.Base;
using Common.Domain.Enums;
using Common.Domain.Exceptions;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly IExpressionBuilder _builder;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DbContext context, IMapper mapper, IAuthServices userServices, IExpressionBuilder builder)
        {
            _ctx = context;
            _map = mapper;
            _usrServices = userServices;
            _builder = builder;
        }

        #region Add
        public async Task<DomainEntity> AddAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            if (entity is IAuditable a)
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
                return entity;
            }
            await _ctx.AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ResultDTO> AddAsync<AddDTO, DomainEntity, ResultDTO>(AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        => _map.Map<ResultDTO>(await AddAsync(_map.Map<DomainEntity>(entity), cancellationToken));

        public async Task<IList<DomainEntity>> AddAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var entities = entity.Select<DomainEntity, IEntity>(e =>
            {
                if (e is IAuditable a)
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
                    return a;
                }
                return e;
            });
            await _ctx.AddRangeAsync(entities);
            await _ctx.SaveChangesAsync();
            return (IList<DomainEntity>)entities;
        }

        public async Task<IList<ResultDTO>> AddAsync<AddDTO, DomainEntity, ResultDTO>(IList<AddDTO> entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var entities = _map.Map<IList<DomainEntity>>(entity);
            await AddAsync(entities, cancellationToken);
            return _map.Map<IList<ResultDTO>>(entities);
        }

        #endregion

        #region Update
        public async Task<DomainEntity> UpdateAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            if (entity is IIdentifiable iE)
            {
                if (!await ExistsAsync<DomainEntity>(iE.Id, cancellationToken))
                    throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Update, iE.Id);
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
                return entity;
            }
            _ctx.Update(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ResultDTO> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
            => _map.Map<ResultDTO>(await UpdateAsync(_map.Map<DomainEntity>(entity), cancellationToken));
        public async Task<IList<DomainEntity>> UpdateAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            if (entity is IList<IEntity> iE)
            {
                var ids = _map.Map<IList<Guid>>(iE);
                var (all, notFoundIds) = await ExistsAsync<DomainEntity>(ids, cancellationToken);
                if (!all) throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Update, notFoundIds);
            }

            var entities = entity.Select<DomainEntity, IEntity>(e =>
            {
                if (e is IAuditable a)
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
                    return a;
                }
                return e;
            });
            _ctx.UpdateRange(entities);
            await _ctx.SaveChangesAsync(cancellationToken);
            return (IList<DomainEntity>)entities;
        }
        public async Task<IList<ResultDTO>> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(IList<UpdateDTO> entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var entities = _map.Map<IList<DomainEntity>>(entity);
            await UpdateAsync(entities, cancellationToken);
            return _map.Map<IList<ResultDTO>>(entities);
        }
        #endregion

        #region Delete
        public async Task DeleteAsync<DomainEntity>(Guid id, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            try
            {
                var entity = await _ctx.Set<DomainEntity>().FirstAsync(x => ((IIdentifiable)x).Id.Equals(id), cancellationToken);
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
                throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Delete, id, ex);
            }
        }

        public async Task DeleteAsync<DomainEntity>(IList<Guid> ids, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            var (all, notFoundIds) = await ExistsAsync<DomainEntity>(ids, cancellationToken);
            if (!all) throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Update, notFoundIds);
            var currentTime = DateTime.UtcNow;
            switch (typeof(DomainEntity))
            {
                case IAuditable:
                    await _ctx.Set<DomainEntity>()
                        .AsNoTracking()
                        .Where(x => ids.Contains(((IIdentifiable)x).Id))
                        .ExecuteUpdateAsync(u => u
                            .SetProperty(x => ((IAuditable)x).DeletedAt, x => currentTime)
                            .SetProperty(x => ((IAuditable)x).DeletedById, x => _usrServices.Id)
                            , cancellationToken
                        );
                    break;
                default:
                    throw new NotImplementedException($"{typeof(DomainEntity)} not implemented delete action.");
            }
        }

        #endregion

        #region Exists
        public async Task<bool> ExistsAsync<DomainEntity>(Guid id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            switch (typeof(DomainEntity))
            {
                case IIdentifiable:
                    return
                        await _ctx.Set<DomainEntity>().AsNoTracking().Where(x => ((IIdentifiable)x).Id == id).AnyAsync(cancellationToken);
                default: return false;
            }
        }

        public async Task<(bool, IList<Guid>)> ExistsAsync<DomainEntity>(IList<Guid> ids, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            switch (typeof(DomainEntity))
            {
                case IIdentifiable:
                    {
                        var foundAll = await _ctx.Set<DomainEntity>().AsNoTracking().AllAsync(x => ids.Contains(((IIdentifiable)x).Id), cancellationToken);
                        if (foundAll) return (foundAll, Array.Empty<Guid>());
                        var foundIds = await _ctx.Set<DomainEntity>().AsNoTracking().Where(x => ids.Contains(((IIdentifiable)x).Id)).ProjectTo<Guid>(_map.ConfigurationProvider).ToListAsync(cancellationToken);
                        return (foundAll, ids.Where(x => !foundIds.Contains(x)).ToList());
                    }
                default:
                    return (false, Array.Empty<Guid>());
            }
        }

        public async Task<bool> ExistsAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
            }
            return await querie.AnyAsync(cancellationToken);
        }
        #endregion

        #region SearchOne
        public async Task<DomainEntity> SearchOneAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if(expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
            }
            return await querie.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ResultDTO> SearchOneAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
            }
            return await querie.ProjectTo<ResultDTO>(_map.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region Search
        public async Task<DomainEntity> SearchAsync<DomainEntity>(Guid id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            switch (typeof(DomainEntity))
            {
                case IIdentifiable:
                    return await _ctx.Set<DomainEntity>().AsNoTracking().Where(x => ((IIdentifiable)x).Id == id).FirstOrDefaultAsync(cancellationToken);
                default: return null;
            }
        }
        public async Task<ResultDTO> SearchAsync<DomainEntity, ResultDTO>(Guid id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        => await _ctx.Set<DomainEntity>().AsNoTracking().Where(x => ((IIdentifiable)x).Id == id).ProjectTo<ResultDTO>(_map.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

        public async Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(IList<Guid> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            switch (typeof(DomainEntity))
            {
                case IIdentifiable:
                    return await _ctx.Set<DomainEntity>().AsNoTracking().Where(x => ids.Contains(((IIdentifiable)x).Id)).PaginateAsync<DomainEntity,DomainEntity>(_map, page, pageSize);
                default: return null;
            }
        }

        public async Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(IList<Guid> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        => await _ctx.Set<DomainEntity>().AsNoTracking().Where(x => ids.Contains(((IIdentifiable)x).Id)).PaginateAsync<DomainEntity,ResultDTO>(_map, page, pageSize);


        public async Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        => await _ctx.Set<DomainEntity>().AsNoTracking().Where(where).PaginateAsync<DomainEntity, DomainEntity>(_map, page, pageSize);

        public async Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
             => await _ctx.Set<DomainEntity>().AsNoTracking().Where(where).PaginateAsync<DomainEntity,ResultDTO>(_map,page,pageSize);

        public async Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
            }
            return await querie.PaginateAsync<DomainEntity,DomainEntity>(_map, filters);
        }

        public async Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
            }
            return await querie.PaginateAsync<DomainEntity,ResultDTO>(_map, filters);
        }
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
