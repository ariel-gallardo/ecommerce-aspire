
using Inventory.Application.DTO;
using Inventory.Domain.Entities;
using Mapster;

namespace Inventory.Application.Profiles
{
    
	public class InventoryItemProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<InventoryItemDTO, InventoryItem>().TwoWays();
        }
    }
}
