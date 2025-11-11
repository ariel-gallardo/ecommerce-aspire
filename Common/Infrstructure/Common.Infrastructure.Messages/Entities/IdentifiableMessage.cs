
namespace Common.Infrastructure.Messages.Entities
{
    public class IdentifiableMessage
    {
        public string Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IdentifiableMessage message &&
                   Id == message.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
