using AutoMapper;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Cache.Key;
using Common.Infrastructure.Messages.Entities;
using Common.Infrastructure.Repositories;
using MassTransit;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Security.Infrastructure.Messaging.Consumer
{
    public class LoadPermissionRequestConsumer : Consumer<LoadPermissionRequest>
    {
        public LoadPermissionRequestConsumer(IUnitOfWork unitOfWork, IMapper mapper, ICacheManagerServices cache) : base(unitOfWork, mapper, cache)
        {

        }

        public override async Task OnConsume(ConsumeContext<LoadPermissionRequest> context)
        {
            var request = context.Message;
            var actionName = CacheKeyCommon.PolicyActionName(request.Controller, request.Action);
            var actionNameCreated = CacheKeyCommon.PolicyActionNameCreated(request.Controller, request.Action);
            var policy = await _cache.GetAsync<string>(actionName);
            if (string.IsNullOrEmpty(policy))
            {
                if(await _unitOfWork.ExistsAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default))
                {
                    var permission = await _unitOfWork.SearchOneAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default);
                    await _cache.SaveAsync(actionName, permission.Policy);
                    await _cache.SaveAsync(actionNameCreated, true);
                }
            }
        }
    }
}
