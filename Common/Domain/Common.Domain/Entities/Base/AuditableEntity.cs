using Common.Domain.Contracts.Entities;

namespace Common.Domain.Entities.Base
{
    public class AuditableEntity : IdentifiableEntity, IAuditable
    {
        public ulong CreatedById { get; set; }
        public ulong? UpdatedById { get; set; }
        public ulong? DeletedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
