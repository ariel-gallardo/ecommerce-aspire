using Common.Application.DTOS.Entities.User;
using Common.Contracts;
using Common.Domain.DTOS.Base.Entities;

namespace Common.Application.Contracts.Services
{
    public interface IUserServices : IScoped
    {
        Task<Response> AuthUser(UserLoginDTO dto, CancellationToken cancellationToken);
        Task<Response> RegisterUser(UserRegisterDTO dto, CancellationToken cancellationToken);
    }
}
