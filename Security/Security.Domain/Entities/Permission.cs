using Common.Domain.Entities.Base;

namespace Security.Domain.Entities
{
    public class Permission : AuditableEntity
    {
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Policy { get; set; }
    }
}
