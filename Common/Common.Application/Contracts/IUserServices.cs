using Application.DTOS.Entities.User;
using Common.Domain.DTOS.Base.Entities;

namespace Application.Contracts.Services
{
    public interface IUserServices
    {
        Task<Response> AuthUser(UserLoginDTO dto, CancellationToken cancellationToken);
    }
}
