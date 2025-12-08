using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;

namespace Common.Application.Profiles.Base
{
    public class IdentifiableProfile : Profile
    {
        public IdentifiableProfile()
        {

            CreateMap<IdentifiableDTO, IdentifiableEntity>().ReverseMap();
            CreateMap<IdentifiableEntity, ulong>().ConvertUsing(entity => entity.Id);
            CreateMap<ulong, IdentifiableEntity>().ConvertUsing(id => new IdentifiableEntity { Id = id });


            CreateMap<IdentifiableGuidDTO, IdentifiableGuidEntity>().ReverseMap();
            CreateMap<IdentifiableGuidEntity, Guid>().ConvertUsing(entity => entity.Id);
            CreateMap<Guid, IdentifiableGuidEntity>().ConvertUsing(id => new IdentifiableGuidEntity { Id = id });
        }
    }
}
