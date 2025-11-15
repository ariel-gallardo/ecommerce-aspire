using Common.Infrastructure.Messages.Entities;

namespace Security.Infrastructure.Messaging.Messages.Request
{
    public class LoadPermissionRequest : AuditableMessage
    {
        public string Url { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}
