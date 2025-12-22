using Logs.Domain.Entities;
using Logs.Infrastructure.Messaging.Request;
using Mapster;

namespace Logs.Application.Profiles
{
    public class LogErrorProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<LogError, DTO.LogErrorDTO>()
                .TwoWays();
            config.NewConfig<LogErrorRequest, LogError>();
        }
    }
}
