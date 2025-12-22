using Common.Domain.Entities.Base;

namespace Logs.Domain.Entities
{
    public class Log : AuditableGuidEntity
    {
        public Guid? ErrorId { get; set; }
        public virtual LogError? Error { get; set; }
    }
}
