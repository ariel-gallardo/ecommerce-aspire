using AutoMapper;
using System.Security.Claims;
using Security.Application.DTO;
using Security.Application.Profiles.Resolvers;
using Security.Domain.Const;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.Rol.ToString()))
                .ReverseMap();
            CreateMap<UserLoginDTO, UserQuerieFilter>();
            CreateMap<UserRegisterDTO, UserQuerieFilter>();
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom<PasswordHashResolver>())
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.RePassword, opt => opt.Ignore());

            #region Claims
            CreateMap<User, Claim[]>()
                .ConvertUsing((user, ctx) =>
                {
                    var claims = new List<Claim>
                    {
                        new Claim(Claims.Role, user.Rol.ToString()),
                        new Claim(Claims.Name, user.Username),
                        new Claim(Claims.Email, user.Email)
                    };

                    if (user.Persona != null)
                    {
                        claims.Add(new Claim(Claims.GivenName, user.Persona.Name));
                        claims.Add(new Claim(Claims.Surname, user.Persona.Lastname));

                        if (user.Persona.Address != null)
                        {
                            claims.Add(new Claim(Claims.Street, user.Persona.Address.Street));
                            claims.Add(new Claim(Claims.Number, user.Persona.Address.Number.ToString()));
                            claims.Add(new Claim(Claims.Neighborhood, user.Persona.Address.Neighborhood));
                            claims.Add(new Claim(Claims.Description, user.Persona.Address.Description));

                            if (user.Persona.Address.Coordinates != null)
                            {
                                claims.Add(new Claim(Claims.Latitude, user.Persona.Address.Coordinates.Latitude.ToString()));
                                claims.Add(new Claim(Claims.Longitude, user.Persona.Address.Coordinates.Longitude.ToString()));
                            }
                        }
                    }
                    return claims.ToArray();
                });
            #endregion
        }
    }
}
