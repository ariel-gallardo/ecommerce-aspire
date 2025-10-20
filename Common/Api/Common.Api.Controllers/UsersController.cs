using Common.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Common.Domain.Filters.Queries;
using Common.Contracts;
using Common.Domain.Entities;
using Common.Api.Controllers.Contracts;
using Common.Application.DTO.Entities.User;
using Common.Application.DTO.Entities.Base;

namespace Common.Api.Controllers
{
    
    public class UsersController : CommonController<User, UserRegisterDTO, UserRegisterDTO, UserDTO,UserQuerieFilter>, IUsersController
    {
        private readonly IUserServices _userServices;

        public UsersController(ICommonServices services, IUserServices userServices) : base(services)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.RegisterUser(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AuthUser(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
