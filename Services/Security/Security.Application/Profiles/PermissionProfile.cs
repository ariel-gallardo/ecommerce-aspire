using Common.Extensions;
using Common.Infrastructure.Entities.Enums;
using Mapster;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Application.Profiles
{
	public class PermissionProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        
        {
            config.NewConfig<PermissionDTO, Permission>()
                .Map(dest => dest.Policy, src => src.Policy.AsEnumUsingMemberValue<Policy>());

            config.NewConfig<Permission,PermissionDTO>()
                .Map(dest => dest.Policy, src => src.Policy.AsStringUsingMemberValue());

            config.NewConfig<PermissionDTO, PermissionQuerieFilter>().TwoWays();
        }
    }
}
