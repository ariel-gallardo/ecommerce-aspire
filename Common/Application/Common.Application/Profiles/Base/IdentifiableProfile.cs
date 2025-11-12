using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;
using Common.Infrastructure.Messages.Entities;

namespace Common.Application.Profiles.Base
{
    public class IdentifiableProfile : Profile
    {
        public IdentifiableProfile()
        {
            CreateMap<IdentifiableDTO, IdentifiableEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<IdentifiableDTO, Guid>()
                .ConvertUsing(src => Guid.Parse(src.Id));

            CreateMap<IdentifiableEntity, Guid>()
                .ConvertUsing(src => src.Id);
            CreateMap<Guid, IdentifiableEntity>()
                .ConvertUsing(guid => new IdentifiableEntity { Id = guid });

            CreateMap<IdentifiableEntity, IdentifiableMessage>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
        }
    }
}
