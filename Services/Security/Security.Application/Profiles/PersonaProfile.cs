using AutoMapper;
using Common.Domain.Entities;
using Security.Application.DTO;

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
