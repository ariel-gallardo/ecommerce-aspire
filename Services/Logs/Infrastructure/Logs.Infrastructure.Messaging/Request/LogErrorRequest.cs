using Common.Infrastructure.Messages.Entities;

namespace Logs.Infrastructure.Messaging.Request
{
    public class LogErrorRequest : Message
    {
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionType { get; set; }
        public LogRequest Log { get; set; }
    }
}
