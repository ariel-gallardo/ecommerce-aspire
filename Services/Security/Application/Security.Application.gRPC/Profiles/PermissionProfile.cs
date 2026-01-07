using Mapster;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.gRPC.Protos;

namespace Security.Application.gRPC.Profiles
{
    public class PermissionProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PermissionRequest, PermissionQuerieFilter>().TwoWays();
            config.NewConfig<PermissionRequest, Permission>().TwoWays();
        }
    }
}
