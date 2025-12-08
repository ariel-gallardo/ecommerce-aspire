using Common.Contracts.Entities;

namespace Common.Domain.Contracts.Entities
{
    public interface IAuditable : IIdentifiable
    {
        ulong CreatedById { get; set; }
        ulong? UpdatedById { get; set; }
        ulong? DeletedById { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
