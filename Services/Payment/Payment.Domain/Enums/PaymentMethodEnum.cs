using System.Text.Json.Serialization;

namespace Payment.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod
    {
        Efectivo,
        MercadoPago,
        Transferencia
    }
}
