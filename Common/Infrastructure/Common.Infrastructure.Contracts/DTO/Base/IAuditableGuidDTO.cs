using System.Text.Json.Serialization;

namespace Common.Infrastructure.Contracts
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
