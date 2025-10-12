using Common.Domain.Contracts.Entities;

namespace Common.Domain.Entities.Base
{
    public class AuditableEntity : IdentifiableEntity, IAuditable
    {
        public Guid Id { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? DeletedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual User DeletedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
