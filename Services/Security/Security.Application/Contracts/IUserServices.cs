using Common.Contracts;
using Common.Infrastructure.Entities;
using Security.Application.DTO;

namespace Security.Application.Contracts.Services
{
    public interface IUserServices : IScoped
    {
        Task<BaseResponse> AuthUser(UserLoginDTO dto, CancellationToken cancellationToken);
        Task<BaseResponse> RegisterUser(UserRegisterDTO dto, CancellationToken cancellationToken);
    }
}
