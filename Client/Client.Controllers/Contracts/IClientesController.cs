using Client.Application.DTO;
using Client.Domain.Entities;
using Client.Domain.Filters;
using Common.Contracts;

namespace Client.Controllers.Contracts
{
    public interface IClientesController : ICommonController<Cliente, ClientDTO, ClientDTO, ClientDTO, ClientQuerieFilter>
    {
    }
}
