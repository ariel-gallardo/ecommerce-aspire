using Common.Domain.DTOS.Entities.User;
using Microsoft.AspNetCore.Mvc;

namespace Impecable.System.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
        {
            return Ok();
        }

        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            return Ok();
        }
    }
}
