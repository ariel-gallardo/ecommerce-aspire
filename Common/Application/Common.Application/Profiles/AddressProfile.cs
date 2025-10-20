using AutoMapper;
using Common.Application.DTO.Entities.Base;
using Common.Domain.ValueObjects;

namespace Common.Application.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
