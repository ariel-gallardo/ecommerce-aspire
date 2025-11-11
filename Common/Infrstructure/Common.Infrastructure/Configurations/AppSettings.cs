namespace Common.Infrastructure.Configurations
{
    public class AppSettings
    {
        public RabbitMQ RabbitMQ { get; set; }
        public Redis Redis { get; set; }
        public JWT Jwt { get; set; }
        public int QuantityToGenerate { get; set; }
    }
}
