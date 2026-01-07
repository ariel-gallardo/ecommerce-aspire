using System.Text.Json.Serialization;

namespace Common.Infrastructure.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Unit
    {
        Unit,
        Gram,
        Kilogram,
        Liter,
        Mililiter,
        USD,
        ARS
    }
}
