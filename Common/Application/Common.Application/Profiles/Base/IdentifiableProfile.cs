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
            CreateMap<IdentifiableDTO, IdentifiableEntity>().ReverseMap();
            CreateMap<IdentifiableDTO, long>();
            CreateMap<IdentifiableEntity, long>();
            CreateMap<long, IdentifiableEntity>()
                .ConvertUsing(id => new IdentifiableEntity { Id = id });

            CreateMap<IdentifiableEntity, IdentifiableMessage>()
                .ReverseMap();
        }
    }
}
