namespace Common.Contracts.DTO.Base
{
    public interface IAuditableDTO : IIdentifiableDTO
    {
        string CreatedAt { get; set; }
        string UpdatedAt { get; set; }
        string DeletedAt { get; set; }
    }
}
