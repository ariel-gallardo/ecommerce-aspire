using AutoMapper;
using Product.Application.DTO;

namespace Product.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Domain.Entities.Product, ProductDTO>().ReverseMap();
        }
    }
}
