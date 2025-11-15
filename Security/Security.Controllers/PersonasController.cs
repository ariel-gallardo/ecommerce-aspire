using Common.Api.Controllers;
using Common.Contracts;
using Security.Application.DTO;
using Security.Controllers.Contracts;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Controllers
{
    public class PersonasController : CommonController<Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>, IPersonasController
    {
        public PersonasController(ICommonServices services) : base(services)
        {
        }
    }
}
