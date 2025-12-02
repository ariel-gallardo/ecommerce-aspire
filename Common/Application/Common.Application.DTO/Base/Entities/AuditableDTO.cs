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
        [JsonIgnore]
        public ulong CreatedById { get; set; }
        [JsonIgnore]
        public ulong? UpdatedById { get; set; }
        [JsonIgnore]
        public ulong? DeletedById { get; set; }
    }
}
