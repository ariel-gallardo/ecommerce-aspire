namespace Common.Infrastructure.Configurations
{
    public class RabbitMQ
    {
        public string DataMount { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ConcurrenctMessageLimit { get; set; }
    }
}
