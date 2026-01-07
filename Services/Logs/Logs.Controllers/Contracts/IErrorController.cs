using Common.Infrastructure.Contracts;
using Logs.Application.DTO;
using Logs.Domain.Entities;
using Logs.Domain.Filters;

namespace Logs.Controllers.Contracts
{
    public interface IErrorController : ICommonController<Guid, LogError, LogErrorDTO, LogErrorDTO, LogErrorDTO, LogErrorQuerieFilter>
    {
    }
}