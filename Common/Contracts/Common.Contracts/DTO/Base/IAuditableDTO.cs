using System.Text.Json.Serialization;

namespace Common.Contracts.DTO.Base
{
    public interface IAuditableDTO : IIdentifiableDTO
    {
        [JsonIgnore]
        string CreatedAt { get; set; }
        [JsonIgnore]
        string UpdatedAt { get; set; }
        [JsonIgnore]
        string DeletedAt { get; set; }
        public ulong CreatedById { get; set; }
        public ulong? UpdatedById { get; set; }
        public ulong? DeletedById { get; set; }
    }
}
