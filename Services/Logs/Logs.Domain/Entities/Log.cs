using Common.Domain.Entities.Base;

namespace Logs.Domain.Entities
{
    public class Log : AuditableGuidEntity
    {
        public string ServiceName { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public Guid? ErrorId { get; set; }
        public virtual LogError? Error { get; set; }
    }
}
