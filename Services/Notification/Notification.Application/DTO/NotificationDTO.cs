using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Notification.Application.DTO
{
    public class NotificationDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string SentAt { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is NotificationDTO dTO &&
                   base.Equals(obj) &&
                   Recipient == dTO.Recipient &&
                   Subject == dTO.Subject &&
                   Message == dTO.Message &&
                   Status == dTO.Status &&
                   Type == dTO.Type &&
                   SentAt == dTO.SentAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Recipient, Subject, Message, Status, Type, SentAt);
        }
    }
}
