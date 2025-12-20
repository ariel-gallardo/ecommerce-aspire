using Mapster;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Security.Application.Profiles
{
	public class PermissionProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<PermissionDTO, Permission>().TwoWays();
            config.NewConfig<PermissionDTO, PermissionQuerieFilter>().TwoWays();
            config.NewConfig<LoadPermissionRequest, PermissionQuerieFilter>().TwoWays();
            config.NewConfig<CreatePermissionRequest, PermissionQuerieFilter>().TwoWays();
            config.NewConfig<CreatePermissionRequest, Permission>().TwoWays();
        }
    }
}
