namespace Common.Contracts.Entities
{
    public interface IIdentifiable : IEntity
    {
        ulong Id { get; set; }
    }
}
