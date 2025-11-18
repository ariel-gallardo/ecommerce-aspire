using System.Text.Json.Serialization;

namespace Notification.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed
    }
}
