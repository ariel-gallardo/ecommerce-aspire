using AutoMapper;
using Security.Application.DTO;
using Security.Application.Profiles.Resolvers;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Security.Application.Profiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionDTO, Permission>().ReverseMap();
            CreateMap<PermissionDTO, PermissionQuerieFilter>();
            CreateMap<LoadPermissionRequest, PermissionQuerieFilter>();
            CreateMap<CreatePermissionRequest, PermissionQuerieFilter>();
            CreateMap<CreatePermissionRequest, Permission>()
                .ForMember(dest => dest.Policy, opt => opt.MapFrom<PolicyResolver>());
        }
    }
}
