using System.Text.Json.Serialization;

namespace Security.Infrastructure.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Client,
        Support,
        Administrator
    }
}
