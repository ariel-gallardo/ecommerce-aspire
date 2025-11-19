namespace Common.Infrastructure.Configurations
{
    public class AppSettings
    {
        public string DatabaseTestingPath { get; set; }
        public RabbitMQ RabbitMQ { get; set; }
        public Redis Redis { get; set; }
        public JWT Jwt { get; set; }
        public ClientDb ClientDb { get; set; }
        public SecurityDb SecurityDb { get; set; }
        public int QuantityToGenerate { get; set; }
    }
}
