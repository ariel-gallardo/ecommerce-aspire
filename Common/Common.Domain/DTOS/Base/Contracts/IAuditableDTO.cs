namespace Common.Domain.DTOS.Base.Contracts
{
    public interface IAuditableDTO : IIdentifiableDTO
    {
        string CreatedById { get; set; }
        string UpdatedById { get; set; }
        string DeletedById { get; set; }
        string CreatedAt { get; set; }
        string UpdatedAt { get; set; }
        string DeletedAt { get; set; }
    }
}
