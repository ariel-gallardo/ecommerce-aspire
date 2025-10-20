using Common.Api.Controllers.Contracts;
using Common.Application.DTO.Entities.Base;
using Common.Contracts;
using Common.Domain.Entities;
using Common.Domain.Filters.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers
{
    [Route("api/[controller]")]
    public class PersonasController : CommonController<Persona, PersonaDTO, PersonaDTO, PersonaDTO,PersonaQuerieFilter>, IPersonasController
    {
        public PersonasController(ICommonServices services) : base(services)
        {
        }
    }
}
