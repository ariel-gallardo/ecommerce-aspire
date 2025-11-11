using AutoMapper;
using Cart.Application.DTO;
using CartEntity = Cart.Domain.Entities.Cart;

namespace Cart.Application.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartDTO, CartEntity>().ReverseMap();
        }
    }
}
