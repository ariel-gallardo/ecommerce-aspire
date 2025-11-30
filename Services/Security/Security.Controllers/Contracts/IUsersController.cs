using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Controllers.Contracts
{
    public interface IUsersController : ICommonController<User, UserRegisterDTO, UserRegisterDTO, UserDTO,UserQuerieFilter>
    {
        
        Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken);

        Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken);
    }
}
