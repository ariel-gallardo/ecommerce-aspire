using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;

namespace Logs.Application.DTO
{
    public class LogErrorDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
    }
}
