
namespace Common.Infrastructure.Messages.Entities
{
    public class AuditableGuidMessage : IdentifiableGuidMessage
    {
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
    }
}
