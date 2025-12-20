
using Common.Application.DTO.ValueObjects;
using Common.Domain.ValueObjects;
using Mapster;

namespace Common.Application.Profiles
{
    
	public class AddressProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<Address, AddressDTO>().TwoWays();
        }
    }
}
