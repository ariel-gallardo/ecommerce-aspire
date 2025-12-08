using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;

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
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom((src, dest) => dest.CreatedById))
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom((src, dest) => dest.UpdatedById))
                .ForMember(dest => dest.DeletedById, opt => opt.MapFrom((src, dest) => dest.DeletedById))
                .ReverseMap()
                .IncludeBase<IdentifiableEntity,IdentifiableDTO>();

            CreateMap<AuditableGuidDTO, AuditableGuidEntity>()
                .IncludeBase<IdentifiableGuidDTO, IdentifiableGuidEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom((src, dest) => dest.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom((src, dest) => dest.UpdatedAt))
                .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom((src, dest) => dest.DeletedAt))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom((src, dest) => dest.CreatedById))
                .ForMember(dest => dest.UpdatedById, opt => opt.MapFrom((src, dest) => dest.UpdatedById))
                .ForMember(dest => dest.DeletedById, opt => opt.MapFrom((src, dest) => dest.DeletedById))
                .ReverseMap()
                .IncludeBase<IdentifiableGuidEntity, IdentifiableGuidDTO>();
        }
    }
}
