namespace Logs.Infrastructure.Messaging.Request
{
    public class LogRequest
    {
        public string ServiceName { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
    }
}
