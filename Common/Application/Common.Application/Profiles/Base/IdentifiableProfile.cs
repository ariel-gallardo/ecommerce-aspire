
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;
using Mapster;

namespace Common.Application.Profiles.Base
{
    
	public class IdentifiableProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {

            config.NewConfig<IdentifiableDTO, IdentifiableEntity>().TwoWays();
            config.NewConfig<IdentifiableEntity, ulong>()
                .Map(dest => dest, src => src.Id)
                .TwoWays();
            config.NewConfig<IdentifiableGuidDTO, IdentifiableGuidEntity>().TwoWays();
            config.NewConfig<IdentifiableGuidEntity, Guid>()
                .Map(dest => dest, src => src.Id)
                .TwoWays();
        }
    }
}
