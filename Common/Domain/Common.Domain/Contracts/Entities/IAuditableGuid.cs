using Common.Contracts.Entities;

namespace Common.Domain.Contracts.Entities
{
    public interface IAuditableGuid : IIdentifiableGuid
    {
        Guid CreatedById { get; set; }
        Guid? UpdatedById { get; set; }
        Guid? DeletedById { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
