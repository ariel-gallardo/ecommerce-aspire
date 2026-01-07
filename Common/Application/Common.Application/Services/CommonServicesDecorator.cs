using Common.Infrastructure.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Application.Services
{
    public class CommonServicesDecorator : ICommonServices
    {
        private readonly ICommonServices _inner;
        private readonly IServiceProvider _provider;

        public CommonServicesDecorator(ICommonServices inner, IServiceProvider provider)
        {
            _inner = inner;
            _provider = provider;
        }


        public async Task<IResponse> AddAsync<AddDTO, DomainEntity, ResultDTO>(AddDTO entity, CancellationToken cancellationToken)
        where AddDTO : class, IEntityDTO, IAddDTO
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<IAddSingleBehavior<AddDTO, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) entity = await b.OnBeforeAsync(entity, cancellationToken);
            var response = await _inner.AddAsync<AddDTO, DomainEntity, ResultDTO>(entity, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> AddAsync<AddDTO, DomainEntity, ResultDTO>(IList<AddDTO> entities, CancellationToken cancellationToken)
        where AddDTO : class, IEntityDTO, IAddDTO
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<IAddBulkBehavior<AddDTO, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) entities = await b.OnBeforeAsync(entities, cancellationToken);
            var response = await _inner.AddAsync<AddDTO, DomainEntity, ResultDTO>(entities, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> DeleteAsync<Key, DomainEntity>(Key entityId, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var behaviors = _provider.GetServices<IDeleteSingleBehavior<DomainEntity,Key>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) await b.OnBeforeAsync(entityId, cancellationToken);
            var response = await _inner.DeleteAsync<Key,DomainEntity>(entityId,cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> DeleteAsync<Key, DomainEntity>(IList<Key> entityIds, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        {
            var behaviors = _provider.GetServices<IDeleteBulkBehavior<DomainEntity, Key>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) await b.OnBeforeAsync(entityIds, cancellationToken);
            var response = await _inner.DeleteAsync<Key, DomainEntity>(entityIds, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> SearchAsync<Key, DomainEntity, ResultDTO>(Key entityId, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<ISearchKeySingleBehavior<Key, DomainEntity, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) await b.OnBeforeAsync(entityId, cancellationToken);
            var response = await _inner.SearchAsync<Key, DomainEntity, ResultDTO>(entityId, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> SearchAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<ISearchQuerieBulkBehavior<DomainEntity, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) filters = await b.OnBeforeAsync(filters, cancellationToken);
            var response = await _inner.SearchAsync<DomainEntity, ResultDTO>(filters, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> SearchAsync<Key, DomainEntity, ResultDTO>(IList<Key> entityIds, int page, int pageSize, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<ISearchKeyBulkBehavior<Key, DomainEntity, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) await b.OnBeforeAsync(entityIds, cancellationToken);
            var response = await _inner.SearchAsync<Key, DomainEntity, ResultDTO>(entityIds, page, pageSize, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> SearchFirstAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<ISearchQuerieSingleBehavior<DomainEntity, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) filters = await b.OnBeforeAsync(filters, cancellationToken);
            var response = await _inner.SearchFirstAsync<DomainEntity, ResultDTO>(filters, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(UpdateDTO entity, CancellationToken cancellationToken)
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<IUpdateSingleBehavior<UpdateDTO, DomainEntity, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) entity = await b.OnBeforeAsync(entity, cancellationToken);
            var response = await _inner.UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(entity, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }

        public async Task<IResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(IList<UpdateDTO> entities, CancellationToken cancellationToken)
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where DomainEntity : class, IEntity
        where ResultDTO : class, IEntityDTO, IReadDTO
        {
            var behaviors = _provider.GetServices<IUpdateBulkBehavior<UpdateDTO, DomainEntity, ResultDTO>>().OrderBy(x => x.Order);
            foreach (var b in behaviors) entities = await b.OnBeforeAsync(entities, cancellationToken);
            var response = await _inner.UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(entities, cancellationToken);
            foreach (var b in behaviors) response = await b.OnAfterAsync(response, cancellationToken);
            return response;
        }
    }
}
