using AutoMapper;
using Common.Application.DTO.Base.Entities;
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

            CreateMap<IdentifiableDTO, Guid>()
                .ConvertUsing(src => Guid.Parse(src.Id));

            CreateMap<Guid, IdentifiableEntity>()
                .ConvertUsing(guid => new IdentifiableEntity { Id = guid });
        }
    }
}
