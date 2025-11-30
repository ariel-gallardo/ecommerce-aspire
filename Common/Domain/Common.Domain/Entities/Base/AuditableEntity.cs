using Common.Domain.Contracts.Entities;

namespace Common.Domain.Entities.Base
{
    public class AuditableEntity : IdentifiableEntity, IAuditable
    {
        public ulong Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
