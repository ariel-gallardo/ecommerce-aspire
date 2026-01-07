using Common.Application.DTO.Base.Entities;
using Common.Infrastructure.Contracts;

namespace Logs.Application.DTO
{
    public class LogDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string ServiceName { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public Guid? ErrorId { get; set; }
    }
}
