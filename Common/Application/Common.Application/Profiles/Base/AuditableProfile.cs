
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;
using Mapster;

namespace Common.Application.Profiles.Base
{
	public class AuditableProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<AuditableDTO, AuditableEntity>().TwoWays();
            config.NewConfig<AuditableGuidDTO, AuditableGuidEntity>().TwoWays();
        }
    }
}
