using Client.Application.DTO;
using Client.Controllers.Contracts;
using Client.Domain.Entities;
using Client.Domain.Filters;
using Common.Api.Controllers;
using Common.Contracts;

namespace Client.Controllers
{
    public class ClientesController : CommonController<Cliente, ClientDTO, ClientDTO, ClientDTO, ClientQuerieFilter>, IClientesController
    {
        public ClientesController(ICommonServices services) : base(services)
        {
        }
    }
}
