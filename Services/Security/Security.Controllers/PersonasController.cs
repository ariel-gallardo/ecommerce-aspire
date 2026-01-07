using Common.Api.Controllers;
using Common.Domain.Entities;
using Common.Infrastructure.Contracts;
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
