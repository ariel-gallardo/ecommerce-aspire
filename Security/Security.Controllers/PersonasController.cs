using Common.Api.Controllers;
using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Presentation.Contracts;

namespace Security.Presentation
{
    [Route("api/[controller]")]
    public class PersonasController : CommonController<Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>, IPersonasController
    {
        public PersonasController(ICommonServices services) : base(services)
        {
        }
    }
}
