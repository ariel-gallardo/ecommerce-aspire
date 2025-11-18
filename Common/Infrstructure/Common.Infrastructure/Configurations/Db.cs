namespace Common.Infrastructure.Configurations
{
    public abstract class Db
    {
        public int Port { get; set; }
        public string DataMount { get; set; }
        public string Password { get; set; }
    }
}
