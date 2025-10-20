using AutoMapper;
using Common.Application.DTO.Entities.User;
using Common.Domain.Contracts.Services;
using Common.Domain.Entities;

namespace Common.Application.Profiles.Resolvers
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
