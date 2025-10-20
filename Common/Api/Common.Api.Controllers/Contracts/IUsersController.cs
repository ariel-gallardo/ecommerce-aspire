using Common.Application.DTO.Entities.Base;
using Common.Application.DTO.Entities.User;
using Common.Contracts;
using Common.Domain.Entities;
using Common.Domain.Filters.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers.Contracts
{
    public interface IUsersController : ICommonController<User, UserRegisterDTO, UserRegisterDTO, UserDTO,UserQuerieFilter>
    {
        
        Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken);

        Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken);
    }
}
