using Common.Application.DTO.Entities.Base;
using Common.Contracts;
using Common.Domain.Entities;
using Common.Domain.Filters.Queries;

namespace Common.Api.Controllers.Contracts
{
    public interface IPersonasController : ICommonController<Persona, PersonaDTO, PersonaDTO, PersonaDTO, PersonaQuerieFilter>
    {
    }
}
