using AutoMapper;
using Common.Extensions;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Cache.Key;
using Common.Infrastructure.Entities.Enums;
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
            var res = await _unitOfWork.SearchOneAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default);
            if (await _unitOfWork.ExistsAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default))
            {
                var permission = await _unitOfWork.SearchOneAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default);
                await context.RespondAsync<Message<string>>(new Message<string> { Data = permission.Policy.AsStringUsingMemberValue() });
            }
            else
            {
                await context.RespondAsync<Message<string>>(new Message<string> { Data = null });
            }
        }
    }
}
