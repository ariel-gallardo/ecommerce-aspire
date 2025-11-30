using Common.Contracts;
using Common.Domain.Entities;
using Security.Application.DTO;
using Security.Domain.Filters.Queries;

namespace Security.Controllers.Contracts
{
    public interface IPersonasController : ICommonController<Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>
    {
    }
}
