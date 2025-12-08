namespace Common.Infrastructure.Configurations
{
    public class RabbitMQ
    {
        public int ConcurrenctMessageLimit { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
