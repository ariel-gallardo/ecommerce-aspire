using Common.Domain.DTOS.Base.Contracts;

namespace Common.Domain.DTOS.Base.Entities
{
    public class AuditableDTO : IdentifiableDTO, IAuditableDTO
    {
        public string CreatedById { get; set; }
        public string UpdatedById { get; set; }
        public string DeletedById { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AuditableDTO dTO &&
                   CreatedById == dTO.CreatedById &&
                   UpdatedById == dTO.UpdatedById &&
                   DeletedById == dTO.DeletedById &&
                   CreatedAt == dTO.CreatedAt &&
                   UpdatedAt == dTO.UpdatedAt &&
                   DeletedAt == dTO.DeletedAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CreatedById, UpdatedById, DeletedById, CreatedAt, UpdatedAt, DeletedAt);
        }
    }
}
