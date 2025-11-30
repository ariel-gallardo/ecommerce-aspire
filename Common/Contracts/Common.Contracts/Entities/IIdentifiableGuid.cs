namespace Common.Contracts.Entities
{
    public interface IIdentifiableGuid : IEntity
    {
        Guid Id { get; set; }
    }
}
