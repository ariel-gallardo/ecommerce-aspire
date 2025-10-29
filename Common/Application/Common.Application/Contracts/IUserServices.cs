using Common.Application.DTO.Entities.User;
using Common.Contracts;
using Common.Infrastructure.Entities;

namespace Common.Application.Contracts.Services
{
    public interface IUserServices : IScoped
    {
        Task<BaseResponse> AuthUser(UserLoginDTO dto, CancellationToken cancellationToken);
        Task<BaseResponse> RegisterUser(UserRegisterDTO dto, CancellationToken cancellationToken);
    }
}
