
namespace Common.Infrastructure.Messages.Entities
{
    public class AuditableMessage : IdentifiableMessage
    {
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AuditableMessage message &&
                   base.Equals(obj) &&
                   CreatedAt == message.CreatedAt &&
                   UpdatedAt == message.UpdatedAt &&
                   DeletedAt == message.DeletedAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), CreatedAt, UpdatedAt, DeletedAt);
        }
    }
}
