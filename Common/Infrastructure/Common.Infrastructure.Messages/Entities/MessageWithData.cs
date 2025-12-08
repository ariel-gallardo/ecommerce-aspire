namespace Common.Infrastructure.Messages.Entities
{
    public class Message<T> : Message
    {
        public T Data { get; set; }
    }
}
