using Common.Api.Controllers;
using Common.Infrastructure.Contracts;
using Logs.Application.DTO;
using Logs.Controllers.Contracts;
using Logs.Domain.Entities;
using Logs.Domain.Filters;

namespace Logs.Controllers
{
    public class ErrorController : CommonController<Guid, LogError, LogErrorDTO, LogErrorDTO, LogErrorDTO, LogErrorQuerieFilter>, IErrorController
    {
        public ErrorController(ICommonServices services) : base(services)
        {
        }
    }
}
