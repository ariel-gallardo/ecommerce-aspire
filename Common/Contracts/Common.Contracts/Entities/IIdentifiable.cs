namespace Common.Contracts.Entities
{
    public interface IIdentifiable : IEntity
    {
        long Id { get; set; }
    }
}
