
using Common.Application.DTO.ValueObjects;
using Common.Domain.ValueObjects;
using Mapster;

namespace Common.Application.Profiles
{
    
	public class CoordinatesProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<Coordinates, CoordinatesDTO>().TwoWays();
        }
    }
}
