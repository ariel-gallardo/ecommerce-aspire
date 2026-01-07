namespace Common.Infrastructure.Contracts
{
    public interface IIdentifiable : IEntity
    {
        ulong Id { get; set; }
    }
}
