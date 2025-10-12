using AutoMapper;
using Common.Domain.DTOS.Base.Entities;
using Common.Domain.Entities.Base;

namespace Common.Application.Profiles.Base
{
    public class IdentifiableProfile : Profile
    {
        public IdentifiableProfile()
        {
            CreateMap<IdentifiableDTO, IdentifiableEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<IdentifiableEntity, IdentifiableDTO>();
        }
    }
}
