using Common.Application.DTOS.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Impecable.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PersonaDTO dto)
        {

            return Ok();
        }

    }
}
