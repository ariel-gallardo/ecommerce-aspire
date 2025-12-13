using AutoMapper;
using Product.Application.DTO;
using Product.Domain.Entities;

namespace Product.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();
        }
    }
}
