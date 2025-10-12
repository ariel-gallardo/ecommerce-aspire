using AutoMapper;
using Common.Application.DTOS.Entities.User;
using System.Security.Claims;
using Common.Domain.Filters.Queries;
using Common.Domain.Entities;
using Common.Domain.Enums;

namespace Common.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserLoginDTO, UserQuerieFilters>();

            #region Claims
            CreateMap<User, Claim[]>()
                .ConvertUsing((user, ctx) =>
                {
                    var claims = new List<Claim>
                    {
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
