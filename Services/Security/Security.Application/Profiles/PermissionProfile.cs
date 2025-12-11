using AutoMapper;
using Common.Application.DTO.Base.Entities;
using Common.Domain.Entities.Base;
using Common.Extensions;
using Common.Infrastructure.Entities.Enums;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Security.Application.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionDTO, Permission>()
                .IncludeBase<AuditableGuidDTO, AuditableGuidEntity>()
                .ForMember(dest => dest.Policy, opt => opt.MapFrom(orig => orig.Policy.AsEnumUsingMemberValue<Policy>()))
                .ReverseMap()
                .IncludeBase<AuditableGuidEntity,AuditableGuidDTO>()
                .ForMember(dest => dest.Policy, opt => opt.MapFrom(orig => orig.Policy.AsStringUsingMemberValue()));
            CreateMap<PermissionDTO, PermissionQuerieFilter>()
                .ForMember(dest => dest.OrderBy, opt => opt.Ignore())
                .ForMember(dest => dest.Page, opt => opt.Ignore())
                .ForMember(dest => dest.PageSize, opt => opt.Ignore());
            CreateMap<LoadPermissionRequest, PermissionQuerieFilter>()
                 .ForMember(dest => dest.OrderBy, opt => opt.Ignore())
                .ForMember(dest => dest.Page, opt => opt.Ignore())
                .ForMember(dest => dest.PageSize, opt => opt.Ignore());
            CreateMap<CreatePermissionRequest, PermissionQuerieFilter>()
                .ForMember(dest => dest.OrderBy, opt => opt.Ignore())
                .ForMember(dest => dest.Page, opt => opt.Ignore())
                .ForMember(dest => dest.PageSize, opt => opt.Ignore());
            CreateMap<CreatePermissionRequest, Permission>()
                .ForMember(dest => dest.Policy, opt => opt.MapFrom(x => Policy.Unknown))
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedById, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
