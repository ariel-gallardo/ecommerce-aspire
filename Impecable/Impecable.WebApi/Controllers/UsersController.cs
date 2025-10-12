using Common.Application.Contracts.Services;
using Common.Application.DTOS.Entities.User;
using Common.Api.SwaggerExamples.UserLogin;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Impecable.WebApi.Controllers
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
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost("login")]
        [SwaggerRequestExample(typeof(UserLoginDTO), typeof(UserLoginRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserLoginResponseOk))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UserLoginResponseUnauthorized))]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AuthUser(dto, cancellationToken);

            return result.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(result),
                StatusCodes.Status401Unauthorized => Unauthorized(result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

    }
}
