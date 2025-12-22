using Logs.Domain.Entities;
using Logs.Infrastructure.Messaging.Request;
using Mapster;

namespace Logs.Application.Profiles
{
    public class LogProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Log, DTO.LogDTO>()
                .TwoWays();
            config.NewConfig<LogRequest, Log>();
        }
    }
}
