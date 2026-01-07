
using Cart.Application.DTO;
using Cart.Domain.Entities;
using Mapster;

namespace Cart.Application.Profiles
{
    
	public class CartItemProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<CartItemDTO, CartItem>().TwoWays();
        }
    }
}
