using AutoMapper;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Infrastructure.Contracts;

namespace Security.Application.Profiles.Resolvers
{
    public class PasswordHashResolver : IValueResolver<UserRegisterDTO, User, string>
    {
        private readonly IAuthServices _authServices;

        public PasswordHashResolver(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        public string Resolve(UserRegisterDTO source, User destination, string destMember, ResolutionContext context)
        => _authServices.HashPassword(source.Password);
    }
}
