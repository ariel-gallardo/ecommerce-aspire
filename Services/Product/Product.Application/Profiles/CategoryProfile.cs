using AutoMapper;
using Product.Application.DTO;
using Product.Domain.Entities;

namespace Product.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
