
using Cart.Application.DTO;
using CartEntity = Cart.Domain.Entities.Cart;
using Mapster;

namespace Cart.Application.Profiles
{
    
	public class CartProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<CartDTO, CartEntity>().TwoWays();
        }
    }
}
