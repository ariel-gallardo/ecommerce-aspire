using Common.Infrastructure.Cache;
using Common.Infrastructure.Contracts;
using Common.Infrastructure.Messages.Entities;
using Logs.Infrastructure.Messaging.Request;
using Mapster;
using MapsterMapper;
using MassTransit;
using Logs.Domain.Entities;

namespace Logs.Infrastructure.Messaging.Consumer
{
    public class LogErrorRequestConsumer : Consumer<LogErrorRequest>
    {
        public LogErrorRequestConsumer(IUnitOfWork unitOfWork, IMapper mapper, ICacheManagerServices cache) : base(unitOfWork, mapper, cache)
        {
        }

        public override async Task OnConsume(ConsumeContext<LogErrorRequest> context)
        {
            var error = context.Message.Adapt<LogError>(_mapper.Config);
            await _unitOfWork.AddAsync(error, default);
        }
    }
}
