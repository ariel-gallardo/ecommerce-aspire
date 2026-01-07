using Common.Api.Controllers;
using Common.Infrastructure.Contracts;
using Logs.Application.DTO;
using Logs.Controllers.Contracts;
using Logs.Domain.Entities;
using Logs.Domain.Filters;

namespace Logs.Controllers
{
    public class InfoController : CommonController<Guid, Log, LogDTO, LogDTO, LogDTO, LogQuerieFilter>, IInfoController
    {
        public InfoController(ICommonServices services) : base(services)
        {
        }
    }
}
