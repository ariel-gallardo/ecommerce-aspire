using Common.Application.Contracts.Services;
using Common.Application.DTOS.Entities.User;
using Common.Api.SwaggerExamples.UserLogin;
using Microsoft.AspNetCore.Mvc;
using Common.Api.SwaggerExamples.UserRegister;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        [SwaggerRequestExample(typeof(UserRegisterDTO), typeof(UserRegisterRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserRegisterResponseOk))]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.RegisterUser(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        [SwaggerRequestExample(typeof(UserLoginDTO), typeof(UserLoginRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserLoginResponseOk))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UserLoginResponseUnauthorized))]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AuthUser(dto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

    }
}
