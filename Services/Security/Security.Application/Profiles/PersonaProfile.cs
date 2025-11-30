using AutoMapper;
using Security.Application.DTO;
using Security.Domain.Entities;

namespace Security.Application.Profiles
{
    public class PersonaProfile : Profile
    {
        public PersonaProfile()
        {
            CreateMap<Persona,PersonaDTO>().ReverseMap();
        }
    }
}
