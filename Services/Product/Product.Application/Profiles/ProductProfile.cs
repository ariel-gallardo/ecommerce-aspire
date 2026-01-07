
using Mapster;
using Product.Application.DTO;

namespace Product.Application.Profiles
{
    
	public class ProductProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        
        {
            config.NewConfig<Domain.Entities.Product, ProductDTO>().TwoWays();
        }
    }
}
