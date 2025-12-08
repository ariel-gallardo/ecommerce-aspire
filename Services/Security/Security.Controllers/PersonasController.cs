using Common.Api.Controllers;
using Common.Contracts;
using Common.Domain.Entities;
using Security.Application.DTO;
using Security.Controllers.Contracts;
using Security.Domain.Filters.Queries;

namespace Security.Controllers
{
    public class PersonasController : CommonController<Guid, Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>, IPersonasController
    {
        public PersonasController(ICommonServices services) : base(services)
        {
        }
    }
}
