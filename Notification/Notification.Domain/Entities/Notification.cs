using Common.Domain.Entities.Base;
using Notification.Domain.Enums;

namespace Notification.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public NotificationType Type { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
