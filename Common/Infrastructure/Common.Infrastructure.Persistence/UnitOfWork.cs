using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Common.Infrastructure.Contracts;
using Security.Infrastructure.Contracts;
using MapsterMapper;
using Mapster;
using Common.Infrastructure.Persistence.Enums;
using Common.Domain.Exceptions;
using Common.Infrastructure.Persistence.Extensions;
using Common.Infrastructure.Entities;

namespace Common.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _ctx;
        private readonly IAuthServices _usrServices;
        private readonly IExpressionBuilder _builder;
        private readonly IServiceProvider _sp;
        private readonly IMapper _mapper;
        private IDbContextTransaction _transaction;


        public UnitOfWork(DbContext context, IAuthServices userServices, IExpressionBuilder builder, IServiceProvider sp, IMapper mapper)
        {
            _ctx = context;
            _usrServices = userServices;
            _builder = builder;
            _sp = sp;
            _mapper = mapper;
        }

        public DbContext Context => _ctx;

        #region Add
        public async Task<DomainEntity> AddAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            if (entity is IAuditable a)
            {
                a.CreatedAt = DateTime.UtcNow;
                a.CreatedById = _usrServices.Id;
                _ctx.Entry(a).Property(x => x.UpdatedAt).IsModified = false;
                _ctx.Entry(a).Property(x => x.DeletedAt).IsModified = false;
                await _ctx.AddAsync(a,cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
                return entity;
            }
            else if(entity is IAuditableGuid b)
            {
                b.CreatedAt = DateTime.UtcNow;
                b.CreatedById = _usrServices.Id;
                _ctx.Entry(b).Property(x => x.UpdatedAt).IsModified = false;
                _ctx.Entry(b).Property(x => x.DeletedAt).IsModified = false;
                await _ctx.AddAsync(b, cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
                return entity;
            }
            await _ctx.AddAsync(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<ResultDTO> AddAsync<AddDTO, DomainEntity, ResultDTO>(AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        => (await AddAsync(entity.Adapt<DomainEntity>(), cancellationToken)).Adapt<ResultDTO>();

        public async Task<IList<DomainEntity>> AddAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var entities = entity.Select<DomainEntity, IEntity>(e =>
            {
                if (e is IAuditable a)
                {
                    a.CreatedAt = DateTime.UtcNow;
                    a.CreatedById = _usrServices.Id;
                    _ctx.Entry(a).Property(x => x.UpdatedAt).IsModified = false;
                    _ctx.Entry(a).Property(x => x.DeletedAt).IsModified = false;
                    return a;
                }else if(e is IAuditableGuid b)
                {
                    b.CreatedAt = DateTime.UtcNow;
                    b.CreatedById = _usrServices.Id;
                    _ctx.Entry(b).Property(x => x.UpdatedAt).IsModified = false;
                    _ctx.Entry(b).Property(x => x.DeletedAt).IsModified = false;
                    return b;
                }
                    return e;
            });
            await _ctx.AddRangeAsync(entities,cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            return (IList<DomainEntity>)entities;
        }

        public async Task<IList<ResultDTO>> AddAsync<AddDTO, DomainEntity, ResultDTO>(IList<AddDTO> entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var entities = entity.Adapt<IList<DomainEntity>>();
            await AddAsync(entities, cancellationToken);
            return entities.Adapt<IList<ResultDTO>>();
        }

        #endregion

        #region Update
        public async Task<DomainEntity> UpdateAsync<DomainEntity>(DomainEntity entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {

            if (entity is IAuditable)
            {
                (entity as IAuditable).UpdatedAt = DateTime.UtcNow;
                (entity as IAuditable).UpdatedById = _usrServices.Id;
                _ctx.Update(entity);
                await _ctx.SaveChangesAsync(cancellationToken);
                return entity;
            }else if(entity is IAuditableGuid)
            {
                (entity as IAuditableGuid).UpdatedAt = DateTime.UtcNow;
                (entity as IAuditableGuid).UpdatedById = _usrServices.Id;
                _ctx.Update(entity);
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
        {
            DomainEntity dbEntity;
            if (entity is IIdentifiableDTO iE)
            {
                if (!await ExistsAsync<ulong, DomainEntity>(iE.Id, cancellationToken))
                    throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Update, iE.Id.ToString());
                else
                {
                    dbEntity = await _ctx.Set<DomainEntity>().FindAsync(iE.Id);
                    return (await UpdateAsync(entity.Adapt(dbEntity), cancellationToken)).Adapt<ResultDTO>();
                }
            }else if (entity is IIdentifiableGuidDTO iEGuid)
            {
                if (!await ExistsAsync<Guid, DomainEntity>(iEGuid.Id, cancellationToken))
                    throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Update, iEGuid.Id.ToString());
                else
                {
                    dbEntity = await _ctx.Set<DomainEntity>().FindAsync(iEGuid.Id);
                    return (await UpdateAsync(entity.Adapt(dbEntity), cancellationToken)).Adapt<ResultDTO>();
                }
            }
            return null;
        }
        public async Task<IList<DomainEntity>> UpdateAsync<DomainEntity>(IList<DomainEntity> entity, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            IList<ulong> ids;
            if (entity is IList<IEntity> iE)
            {
                ids = iE.Adapt<IList<ulong>>();
                var (all, notFoundIds) = await ExistsAsync<ulong, DomainEntity>(ids, cancellationToken);
                if (!all) throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Update, notFoundIds.Select(x => x.ToString()).ToList());
            }

            var entities = entity.Select<DomainEntity, IEntity>( e =>
            {
                if (e is IAuditable a)
                {
                    var existing = _ctx.Set<DomainEntity>().Find(a.Id);
                    a.UpdatedAt = DateTime.UtcNow;
                    a.UpdatedById = _usrServices.Id;
                    _ctx.Entry(existing).CurrentValues.SetValues(a);
                    _ctx.Entry(existing).Property(x => (x as IAuditable).CreatedAt).IsModified = false;
                    _ctx.Entry(existing).Property(x => (x as IAuditable).DeletedAt).IsModified = false;
                    return existing;
                }
                else if (e is IAuditableGuid b)
                {
                    var existing = _ctx.Set<DomainEntity>().Find(b.Id);
                    b.UpdatedAt = DateTime.UtcNow;
                    b.UpdatedById = _usrServices.Id;
                    _ctx.Entry(existing).CurrentValues.SetValues(b);
                    _ctx.Entry(existing).Property(x => (x as IAuditableGuid).CreatedAt).IsModified = false;
                    _ctx.Entry(existing).Property(x => (x as IAuditableGuid).DeletedAt).IsModified = false;
                    return existing;
                }
                return e;
            });
            _ctx.UpdateRange(entities, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            return (IList<DomainEntity>)entities;
        }
        public async Task<IList<ResultDTO>> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(IList<UpdateDTO> entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var entities = entity.Adapt<IList<DomainEntity>>();
            await UpdateAsync(entities, cancellationToken);
            return entities.Adapt<IList<ResultDTO>>();
        }
        #endregion

        #region Delete
        public async Task DeleteAsync<Key,DomainEntity>(Key id, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
        {
            if (!typeof(IIdentifiable).IsAssignableFrom(typeof(DomainEntity)))
                throw new NotImplementedException($"{typeof(DomainEntity)} does not implement IIdentifiable");

            var entity = await _ctx.Set<DomainEntity>().FirstOrDefaultAsync(x => ((IIdentifiable)x).Id.Equals(id));

            if (entity == null)
                throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Delete, id.ToString());

            if (entity is IAuditable a)
            {
                a.DeletedAt = DateTime.UtcNow;
                a.DeletedById = _usrServices.Id;
                _ctx.Entry(a).Property(x => x.CreatedAt).IsModified = false;
                _ctx.Entry(a).Property(x => x.UpdatedAt).IsModified = false;
                _ctx.Update(a);
            }
            else if (entity is IAuditableGuid b)
            {
                b.DeletedAt = DateTime.UtcNow;
                b.DeletedById = _usrServices.Id;
                _ctx.Entry(b).Property(x => x.CreatedAt).IsModified = false;
                _ctx.Entry(b).Property(x => x.UpdatedAt).IsModified = false;
                _ctx.Update(b);
            }
            else
            {
                _ctx.Remove(entity);
            }

            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync<Key,DomainEntity>(IList<Key> ids, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
        {
            if (!typeof(IIdentifiable).IsAssignableFrom(typeof(DomainEntity)))
                throw new NotImplementedException($"{typeof(DomainEntity)} does not implement IIdentifiable");

            var (all, notFoundIds) = await ExistsAsync<Key, DomainEntity>(ids, cancellationToken);
            if (!all) throw new EntityNotFoundException(typeof(DomainEntity).Name, ActionEnum.Delete, notFoundIds.Select(x => x.ToString()).ToList());

            var set = _ctx.Set<DomainEntity>().AsNoTracking().Cast<IIdentifiable>();
            var currentTime = DateTime.UtcNow;

            if (typeof(IAuditable).IsAssignableFrom(typeof(DomainEntity)))
            {
                await _ctx.Set<DomainEntity>()
                          .Cast<IIdentifiable>()
                          .Where(x => ids.Any(y => y.Equals(x.Id)))
                          .Cast<DomainEntity>()
                          .ExecuteUpdateAsync(u => 
                            u.SetProperty(x => ((IAuditable)x).DeletedAt, x => currentTime)
                            .SetProperty(x => ((IAuditable)x).DeletedById, x => _usrServices.Id)
                          ,cancellationToken);
            }
            else if (typeof(IAuditableGuid).IsAssignableFrom(typeof(DomainEntity)))
            {
                await _ctx.Set<DomainEntity>()
                          .Cast<IIdentifiable>()
                          .Where(x => ids.Any(y => y.Equals(x.Id)))
                          .Cast<DomainEntity>()
                          .ExecuteUpdateAsync(u =>
                            u.SetProperty(x => ((IAuditableGuid)x).DeletedAt, x => currentTime)
                            .SetProperty(x => ((IAuditableGuid)x).DeletedById, x => _usrServices.Id)
                          , cancellationToken);
            }
            else
            {
                var entities = await _ctx.Set<DomainEntity>()
                                         .Cast<IIdentifiable>()
                                         .Where(x => ids.Any(y => y.Equals(x.Id)))
                                         .Cast<DomainEntity>()
                                         .ToListAsync(cancellationToken);
                _ctx.RemoveRange(entities,cancellationToken);
                await _ctx.SaveChangesAsync(cancellationToken);
            }
        }

        #endregion

        #region Exists
        public async Task<bool> ExistsAsync<Key,DomainEntity>(Key id, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
        {
            if (typeof(IIdentifiable).IsAssignableFrom(typeof(DomainEntity)))
            {
                return await _ctx.Set<DomainEntity>()
                                 .AsNoTracking()
                                 .Cast<IIdentifiable>()
                                 .AnyAsync(x => x.Id.Equals(id),cancellationToken);
            }
            else if (typeof(IIdentifiableGuid).IsAssignableFrom(typeof(DomainEntity)))
            {
                return await _ctx.Set<DomainEntity>()
                                 .AsNoTracking()
                                 .Cast<IIdentifiableGuid>()
                                 .AnyAsync(x => x.Id.Equals(id), cancellationToken);
            }
            return false;
        }

        public async Task<(bool, IList<Key>)> ExistsAsync<Key,DomainEntity>(IList<Key> ids, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
        {
            if (typeof(IIdentifiable).IsAssignableFrom(typeof(DomainEntity)))
            {
                var set = _ctx.Set<DomainEntity>().AsNoTracking().Cast<IIdentifiable>();
                var foundAll = await set.AllAsync(x => ids.Any(y => y.Equals(x.Id)));

                if (foundAll) return (true, Array.Empty<Key>());

                var foundIds = await set.Where(x => ids.Any(y => y.Equals(x.Id)))
                                        .Select(x => x.Id)
                                        .ToListAsync(cancellationToken);

                return (false, ids.Where(x => !foundIds.Any(y => y.Equals(x))).ToList());
            }
            return (false, Array.Empty<Key>());
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
            var querie = _ctx.Set<DomainEntity>().AsQueryable().IgnoreAutoIncludes();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if(expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
                querie = querie.ApplyModifiers(_sp);
            }
            return await querie.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ResultDTO> SearchOneAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable().IgnoreAutoIncludes();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
                querie = querie.ApplyModifiers(_sp);
            }
            return await querie.ProjectToType<ResultDTO>().FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region Search
        public async Task<DomainEntity> SearchAsync<Key, DomainEntity>(Key id, CancellationToken cancellationToken)
            
            where DomainEntity : class, IEntity
        {
            if (typeof(IIdentifiable).IsAssignableFrom(typeof(DomainEntity)))
            {
                return await _ctx.Set<DomainEntity>()
                                 .IgnoreAutoIncludes()
                                 .AsNoTracking()
                                 .Cast<IIdentifiable>()
                                 .Where(x => x.Id.Equals(id))
                                 .Cast<DomainEntity>()
                                 .ApplyModifiers(_sp)
                                 .FirstOrDefaultAsync(cancellationToken);
            }
            else if (typeof(IIdentifiableGuid).IsAssignableFrom(typeof(DomainEntity)))
            {
                return await _ctx.Set<DomainEntity>()
                                 .IgnoreAutoIncludes()
                                 .AsNoTracking()
                                 .Cast<IIdentifiableGuid>()
                                 .Where(x => x.Id.Equals(id))
                                 .Cast<DomainEntity>()
                                 .ApplyModifiers(_sp)
                                 .FirstOrDefaultAsync(cancellationToken);
            }
            return null;
        }
        public async Task<ResultDTO> SearchAsync<Key, DomainEntity, ResultDTO>(Key id, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            if (typeof(DomainEntity).IsAssignableTo(typeof(IIdentifiable)))
                return await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Cast<IIdentifiable>().Where(x => x.Id.Equals(id)).Cast<DomainEntity>().ApplyModifiers(_sp).ProjectToType<ResultDTO>().FirstOrDefaultAsync(cancellationToken);
            else if(typeof(DomainEntity).IsAssignableTo(typeof(IIdentifiableGuid)))
                return await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Cast<IIdentifiableGuid>().Where(x => x.Id.Equals(id)).Cast<DomainEntity>().ApplyModifiers(_sp).ProjectToType<ResultDTO>().FirstOrDefaultAsync(cancellationToken);
            return null;
        }

        public async Task<IPagedList<DomainEntity>> SearchAsync<Key, DomainEntity>(IList<Key> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            if (typeof(DomainEntity).IsAssignableTo(typeof(IIdentifiable)))
                return await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Cast<IIdentifiable>().Where(x => ids.Cast<ulong>().Contains(x.Id)).ApplyModifiers(_sp).PaginateAsync<DomainEntity>(_mapper,page, pageSize);
            else if (typeof(DomainEntity).IsAssignableTo(typeof(IIdentifiableGuid)))
                return await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Cast<IIdentifiableGuid>().Where(x => ids.Cast<Guid>().Contains(x.Id)).ApplyModifiers(_sp).PaginateAsync<DomainEntity>(_mapper, page, pageSize);
            return new PagedList<DomainEntity>();
        }

        public async Task<IPagedList<ResultDTO>> SearchAsync<Key,DomainEntity, ResultDTO>(IList<Key> ids, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            if (typeof(DomainEntity).IsAssignableTo(typeof(IIdentifiable)))
                return await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Cast<IIdentifiable>().Where(x => ids.Cast<ulong>().Contains(x.Id) ).ApplyModifiers(_sp).PaginateAsync<ResultDTO>(_mapper,page, pageSize);
            else if (typeof(DomainEntity).IsAssignableTo(typeof(IIdentifiableGuid)))
                return await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Cast<IIdentifiableGuid>().Where(x => ids.Cast<Guid>().Contains(x.Id) ).ApplyModifiers(_sp).PaginateAsync<ResultDTO>(_mapper,page, pageSize);
            return new PagedList<ResultDTO>();
        }


        public async Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        => await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Where(where).ApplyModifiers(_sp).PaginateAsync<DomainEntity, DomainEntity>(_mapper, page, pageSize);

        public async Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(Expression<Func<DomainEntity, bool>> where, int page, int pageSize, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
             => await _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsNoTracking().Where(where).ApplyModifiers(_sp).PaginateAsync<DomainEntity,ResultDTO>(_mapper,page, pageSize);

        public async Task<IPagedList<DomainEntity>> SearchAsync<DomainEntity>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var querie = _ctx.Set<DomainEntity>().IgnoreAutoIncludes().AsQueryable();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
                querie = querie.ApplyModifiers(_sp);
            }
            return await querie.PaginateAsync<DomainEntity,DomainEntity>(_mapper, filters);
        }

        public async Task<ResultDTO> SearchFirstAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable().IgnoreAutoIncludes();
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
                querie = querie.ApplyModifiers(_sp);
            }
            return await querie.ProjectToType<ResultDTO>().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IPagedList<ResultDTO>> SearchAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var querie = _ctx.Set<DomainEntity>().AsQueryable().IgnoreAutoIncludes();
            querie = querie.ApplyModifiers(_sp);
            if (filters != null)
            {
                var expressions = _builder.Build<DomainEntity>(filters);
                if (expressions != null)
                    querie = querie.Where(expressions);
                if (!string.IsNullOrWhiteSpace(filters.OrderBy))
                    querie = querie.ApplyOrderBy(filters.OrderBy);
            }
            return await querie.PaginateAsync<DomainEntity,ResultDTO>(_mapper, filters);
        }
        #endregion

        #region Transaction
        public async Task BeginTransaction(CancellationToken cancellationToken)
        {
            _transaction = await _ctx.Database.BeginTransactionAsync();
        }

        public async Task CreateSavePoint(string name, CancellationToken cancellationToken)
        => await _transaction.CreateSavepointAsync(name);

        public async Task RollbackToSavepoint(string name, CancellationToken cancellationToken)
        => await _transaction.RollbackToSavepointAsync(name);

        public async Task ReleaseSavepoint(string name, CancellationToken cancellationToken)
        => await _transaction.ReleaseSavepointAsync(name);

        public async Task CommitTransaction(CancellationToken cancellationToken)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransaction(CancellationToken cancellationToken)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        #endregion
    }
}
