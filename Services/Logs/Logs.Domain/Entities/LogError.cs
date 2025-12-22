using Common.Domain.Entities.Base;

namespace Logs.Domain.Entities
{
    public class LogError : AuditableGuidEntity
    {
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionType { get; set; }
        public virtual Log Log { get; set; }
    }
}
