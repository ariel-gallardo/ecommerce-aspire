using AutoMapper;
using Common.Application.DTOS.Entities;
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
