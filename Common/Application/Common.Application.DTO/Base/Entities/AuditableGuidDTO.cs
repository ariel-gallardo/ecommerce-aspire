using Common.Contracts.DTO.Base;
using System.Text.Json.Serialization;

namespace Common.Application.DTO.Base.Entities
{
    public class AuditableGuidDTO : IdentifiableGuidDTO, IAuditableGuidDTO
    {
        [JsonIgnore]
        public string CreatedAt { get; set; }
        [JsonIgnore]
        public string UpdatedAt { get; set; }
        [JsonIgnore]
        public string DeletedAt { get; set; }
    }
}
