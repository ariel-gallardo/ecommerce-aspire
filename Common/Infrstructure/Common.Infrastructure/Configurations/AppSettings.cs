namespace Common.Infrastructure.Configurations
{
    public class AppSettings
    {
        public string DatabaseDevPath { get; set; }
        public RabbitMQ RabbitMQ { get; set; }
        public Redis Redis { get; set; }
        public JWT Jwt { get; set; }
        public ClientDb ClientDb { get; set; }
        public SecurityDb SecurityDb { get; set; }
        public int QuantityToGenerate { get; set; }
    }
}
