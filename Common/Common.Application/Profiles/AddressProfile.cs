using AutoMapper;
using Application.DTOS.Entities;
using Common.Domain.ValueObjects;

namespace Base.Application.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
