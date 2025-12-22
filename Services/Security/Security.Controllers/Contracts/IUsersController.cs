using Common.Domain.Entities;
using Common.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Security.Application.DTO;
using Security.Domain.Filters.Queries;

namespace Security.Controllers.Contracts
{
    public interface IUsersController : ICommonController<ulong, User, UserRegisterDTO, UserRegisterDTO, UserDTO,UserQuerieFilter>
    {
        
        Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken);

        Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken);
    }
}
