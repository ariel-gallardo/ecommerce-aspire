using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;
using Common.Infrastructure.Messages.Entities;

namespace Common.Application.Profiles.Base
{
    public class AuditableProfile : Profile
    {
        public AuditableProfile()
        {
            CreateMap<AuditableEntity, AuditableDTO>()
                .ForMember(dest => dest.UpdatedAt, opt =>
                {
                    opt.PreCondition(orig => orig.UpdatedAt != null);
                    opt.MapFrom(orig => orig.UpdatedAt);
                })
                .ForMember(dest => dest.DeletedAt, opt =>
                {
                    opt.PreCondition(orig => orig.DeletedAt != null);
                    opt.MapFrom(orig => orig.DeletedAt);
                })
                .ReverseMap()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedById, opt => opt.Ignore());
            CreateMap<AuditableEntity, AuditableMessage>();
        }
    }
}
