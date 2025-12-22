using Common.Infrastructure.Contracts;
using Logs.Application.DTO;
using Logs.Domain.Entities;
using Logs.Domain.Filters;

namespace Logs.Controllers.Contracts
{
    public interface IInfoController : ICommonController<Guid, Log, LogDTO, LogDTO, LogDTO, LogQuerieFilter>
    {
    }
}