using AutoMapper;
using Cart.Application.DTO;
using Cart.Domain.Entities;

namespace Cart.Application.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItemDTO, CartItem>().ForMember(dest => dest.Cart, opt => opt.Ignore()).ReverseMap();
        }
    }
}
