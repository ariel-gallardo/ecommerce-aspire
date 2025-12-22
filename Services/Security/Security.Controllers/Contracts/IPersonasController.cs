using Common.Domain.Entities;
using Common.Infrastructure.Contracts;
using Security.Application.DTO;
using Security.Domain.Filters.Queries;

namespace Security.Controllers.Contracts
{
    public interface IPersonasController : ICommonController<Guid, Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>
    {
    }
}
