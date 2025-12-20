
using Common.Domain.Entities;
using Mapster;
using Security.Application.DTO;

namespace Security.Application.Profiles
{
    
	public class PersonaProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<Persona,PersonaDTO>().TwoWays();
        }
    }
}
