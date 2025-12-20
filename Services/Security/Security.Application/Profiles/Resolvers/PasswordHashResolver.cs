
using Common.Domain.Entities;
using Security.Application.DTO;
using Security.Infrastructure.Contracts;

namespace Security.Application.Profiles.Resolvers
{
    public class PasswordHashResolver
    {
        private readonly IAuthServices _authServices;

        public PasswordHashResolver(IAuthServices authServices)
        {
            _authServices = authServices;
        }
    }
}
