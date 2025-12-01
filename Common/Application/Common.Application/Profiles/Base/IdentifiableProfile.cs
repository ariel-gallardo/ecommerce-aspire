using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Domain.Entities.Base;
using Common.Infrastructure.Messages.Entities;

namespace Common.Application.Profiles.Base
{
    public class IdentifiableProfile : Profile
    {
        public IdentifiableProfile()
        {
            CreateMap<IdentifiableDTO, IdentifiableEntity>()
                .ReverseMap();
            CreateMap<IdentifiableDTO, ulong>();
            CreateMap<IdentifiableEntity, ulong>();
            CreateMap<IdentifiableDTO, ulong>();
            CreateMap<IIdentifiable, ulong>();
            CreateMap<ulong, IdentifiableEntity>()
                .ConvertUsing(id => new IdentifiableEntity { Id = id });
            CreateMap<IdentifiableEntity, IdentifiableMessage>()
                .ReverseMap();

            CreateMap<IdentifiableGuidDTO, IdentifiableGuidEntity>()
            .ReverseMap();

            CreateMap<IdentifiableGuidDTO, Guid>();
            CreateMap<IdentifiableGuidEntity, Guid>();
            CreateMap<IdentifiableGuidDTO, Guid>();
            CreateMap<IIdentifiableGuid, Guid>();
            CreateMap<Guid, IdentifiableGuidEntity>()
                .ConvertUsing(id => new IdentifiableGuidEntity { Id = id });
            CreateMap<IdentifiableGuidEntity, IdentifiableGuidMessage>()
                .ReverseMap();
        }
    }
}
