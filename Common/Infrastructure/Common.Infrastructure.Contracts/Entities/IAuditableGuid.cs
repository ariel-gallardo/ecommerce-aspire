namespace Common.Infrastructure.Contracts
{
    public interface IAuditableGuid : IIdentifiableGuid
    {
        ulong CreatedById { get; set; }
        ulong? UpdatedById { get; set; }
        ulong? DeletedById { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
