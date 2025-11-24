using AutoMapper;
using Client.Application.DTO;
using Client.Domain.Entities;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;

namespace Client.Application.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientDTO, Cliente>()
                .IncludeBase<AuditableDTO,AuditableEntity>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(orig => 
                    DateTime.ParseExact(orig.FechaNacimiento,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                ))
                .ReverseMap()
                .IncludeBase<AuditableEntity,AuditableDTO>()
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(orig => orig.FechaNacimiento.ToString("dd/MM/yyyy")));

            CreateMap<ClientUpdateDTO, Cliente>()
            .IncludeBase<ClientDTO,Cliente>()
            .ForMember(dest => dest.FechaNacimiento, opt => opt.Ignore())
            .ForMember(dest => dest.Cuit, opt => opt.Ignore())
            .ForMember(dest => dest.RazonSocial,opt => opt.Ignore());
        }
    }
}
