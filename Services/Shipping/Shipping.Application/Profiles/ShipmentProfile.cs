using Mapster;
using Shipping.Application.DTO;
using Shipping.Domain.Entities;

namespace Shipping.Application.Profiles
{
    
	public class ShipmentProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<ShipmentDTO, Shipment>().TwoWays();
        }
    }
}
