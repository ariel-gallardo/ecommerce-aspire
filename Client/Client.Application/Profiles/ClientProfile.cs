using AutoMapper;
using Client.Application.DTO;
using Client.Domain.Entities;

namespace Client.Application.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientDTO, Cliente>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(orig => DateTime.ParseExact(orig.FechaNacimiento,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)))
                .ReverseMap()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(orig => orig.FechaNacimiento.ToString("dd/MM/yyyy")));
        }
    }
}
