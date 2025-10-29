using Common.Api.Controllers.Contracts;
using Common.Application.Contracts.Services;
using Common.Application.DTO.Entities.Base;
using Common.Application.DTO.Entities.User;
using Common.Contracts;
using Common.Domain.Entities;
using Common.Domain.Filters.Queries;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDTO))]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.RegisterUser(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponse))]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AuthUser(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
