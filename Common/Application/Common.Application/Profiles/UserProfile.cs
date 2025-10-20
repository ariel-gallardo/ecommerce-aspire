using AutoMapper;
using Common.Application.DTO.Entities.Base;
using Common.Application.DTO.Entities.User;
using Common.Application.Profiles.Resolvers;
using Common.Domain.Entities;
using Common.Domain.Enums;
using Common.Domain.Filters.Queries;
using System.Security.Claims;

namespace Common.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => Enum.Parse<RoleEnum>(src.Rol)));
            CreateMap<UserLoginDTO, UserQuerieFilter>();
            CreateMap<UserRegisterDTO, UserQuerieFilter>();
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom<PasswordHashResolver>())
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => Enum.Parse<RoleEnum>(src.Rol)))
                .ReverseMap()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToString()));
            #region Claims
            CreateMap<User, Claim[]>()
                .ConvertUsing((user, ctx) =>
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, user.Rol.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    if (user.Persona != null)
                    {
                        claims.Add(new Claim(ClaimTypes.GivenName, user.Persona.Name));
                        claims.Add(new Claim(ClaimTypes.Surname, user.Persona.Lastname));

                        if (user.Persona.Address != null)
                        {
                            claims.Add(new Claim(CustomClaims.Street, user.Persona.Address.Street));
                            claims.Add(new Claim(CustomClaims.Number, user.Persona.Address.Number.ToString()));
                            claims.Add(new Claim(CustomClaims.Neighborhood, user.Persona.Address.Neighborhood));
                            claims.Add(new Claim(CustomClaims.Description, user.Persona.Address.Description));

                            if (user.Persona.Address.Coordinates != null)
                            {
                                claims.Add(new Claim(CustomClaims.Latitude, user.Persona.Address.Coordinates.Latitude.ToString()));
                                claims.Add(new Claim(CustomClaims.Longitude, user.Persona.Address.Coordinates.Longitude.ToString()));
                            }
                        }
                    }
                    return claims.ToArray();
                });
            #endregion
        }
    }
}
