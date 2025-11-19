using Common.Api.CustomAttributes;
using Common.Contracts.DTO.Base;
using System.Text.Json.Serialization;

namespace Common.Application.DTO.Base.Entities
{
    public class AuditableDTO : IdentifiableDTO, IAuditableDTO
    {
        [JsonIgnore]
        public string CreatedAt { get; set; }
        [JsonIgnore]
        public string UpdatedAt { get; set; }
        [JsonIgnore]
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
