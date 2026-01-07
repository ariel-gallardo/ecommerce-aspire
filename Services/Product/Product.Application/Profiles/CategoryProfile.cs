
using Mapster;
using Product.Application.DTO;
using Product.Domain.Entities;

namespace Product.Application.Profiles
{
    
	public class CategoryProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Category, CategoryDTO>()
                .TwoWays()
                .PreserveReference(true)
                .MaxDepth(10);
        }
    }
}
