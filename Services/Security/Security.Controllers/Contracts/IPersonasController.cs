using Common.Contracts;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Controllers.Contracts
{
    public interface IPersonasController : ICommonController<Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>
    {
    }
}
