using System.Text.Json.Serialization;

namespace Common.Contracts.DTO.Base
{
    public interface IAuditableGuidDTO : IIdentifiableGuidDTO
    {
        [JsonIgnore]
        string CreatedAt { get; set; }
        [JsonIgnore]
        string UpdatedAt { get; set; }
        [JsonIgnore]
        string DeletedAt { get; set; }
    }
}
