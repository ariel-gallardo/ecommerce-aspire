using Common.Api.CustomAttributes;
using Common.Contracts.DTO.Base;

namespace Common.Application.DTO.Base.Entities
{
    [IgnoreAuditable]
    public class AuditableDTO : IdentifiableDTO, IAuditableDTO
    {
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AuditableDTO dTO &&
                   CreatedAt == dTO.CreatedAt &&
                   UpdatedAt == dTO.UpdatedAt &&
                   DeletedAt == dTO.DeletedAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), CreatedAt, UpdatedAt, DeletedAt);
        }
    }
}
