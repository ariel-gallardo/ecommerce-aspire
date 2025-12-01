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
            CreateMap<AuditableDTO, AuditableEntity>()
                .IncludeBase<IdentifiableDTO,IdentifiableEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom((src, dest) => dest.UpdatedAt))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom((src, dest) => dest.DeletedAt))
                .ReverseMap()
                .IncludeBase<IdentifiableEntity,IdentifiableDTO>();

            CreateMap<AuditableGuidDTO, AuditableGuidEntity>()
                .IncludeBase<IdentifiableGuidDTO, IdentifiableGuidEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom((src, dest) => dest.UpdatedAt))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom((src, dest) => dest.DeletedAt))
                .ReverseMap()
                .IncludeBase<IdentifiableGuidEntity, IdentifiableGuidDTO>();

            CreateMap<AuditableEntity, AuditableMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AuditableGuidEntity, AuditableGuidMessage>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
