using System.Text.Json.Serialization;

namespace Invoice.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        ARS,
        USD
    }
}
