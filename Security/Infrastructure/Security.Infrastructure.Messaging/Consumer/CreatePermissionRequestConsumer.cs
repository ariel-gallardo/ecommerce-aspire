using AutoMapper;
using Common.Infrastructure.Cache;
using Common.Infrastructure.Messages.Entities;
using Common.Infrastructure.Repositories;
using MassTransit;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.Contracts;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Security.Infrastructure.Messaging.Consumer
{
    public class CreatePermissionRequestConsumer : Consumer<CreatePermissionRequest>
    {
        private readonly IAuthServices _authServices;

        public CreatePermissionRequestConsumer(IUnitOfWork unitOfWork, IMapper mapper, ICacheManagerServices cache, IAuthServices authServices) : base(unitOfWork, mapper, cache)
        {
            _authServices = authServices;
        }

        public override async Task OnConsume(ConsumeContext<CreatePermissionRequest> context)
        {
            var response = new Message<string>();
            await _authServices.AuthAsAdmin();
            var message = context.Message;
            var filters = _mapper.Map<PermissionQuerieFilter>(message);
            if (!await _unitOfWork.ExistsAsync<Permission>(filters, default))
            {
                var permission = _mapper.Map<Permission>(message);
                await _unitOfWork.AddAsync(permission,default);
                response.Data = permission.Policy;
            }
           await context.RespondAsync(response);
        }
    }
}
