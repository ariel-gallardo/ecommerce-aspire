using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.Base;
using Common.Domain.Contracts.Entities;
using Common.Domain.Entities.Base;
using Common.Infrastructure.Messages.Entities;

namespace Common.Application.Profiles.Base
{
    public class AuditableProfile : Profile
    {
        public AuditableProfile()
        {
            CreateMap<AuditableEntity, AuditableDTO>()
                .ForMember(dest => dest.DeletedAt, opt =>
                {
                    opt.PreCondition(orig => orig.DeletedAt != null);
                    opt.MapFrom(orig => orig.DeletedAt);
                })
                .ReverseMap()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());

            CreateMap<IAuditable, IAuditableDTO>()
            .ForMember(dest => dest.DeletedAt, opt =>
            {
                opt.PreCondition(orig => orig.DeletedAt != null);
                opt.MapFrom(orig => orig.DeletedAt);
            })
            .ReverseMap()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
            CreateMap<AuditableEntity, AuditableMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
