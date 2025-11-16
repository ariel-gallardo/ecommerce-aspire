using Common.Infrastructure.Messages.Entities;

namespace Security.Infrastructure.Messaging.Messages.Request
{
    public class CreatePermissionRequest : Message
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
    }
}
