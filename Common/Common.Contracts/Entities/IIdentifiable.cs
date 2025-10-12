namespace Common.Contracts.Entities
{
    public interface IIdentifiable : IEntity
    {
        Guid Id { get; set; }
    }
}
