using AutoMapper;
using Common.Application.DTO.Entities.Base;
using Common.Domain.Entities;

namespace Common.Application.Profiles
{
    public class PersonaProfile : Profile
    {
        public PersonaProfile()
        {
            CreateMap<Persona,PersonaDTO>().ReverseMap();
        }
    }
}
