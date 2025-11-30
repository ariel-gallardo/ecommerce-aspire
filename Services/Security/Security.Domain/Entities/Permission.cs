using Common.Domain.Entities.Base;
using Common.Infrastructure.Entities.Enums;

namespace Security.Domain.Entities
{
    public class Permission : AuditableGuidEntity
    {
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public Policy Policy { get; set; }
    }
}
