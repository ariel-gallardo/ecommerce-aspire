namespace Common.Infrastructure.Contracts
{
    public interface IIdentifiableGuid : IEntity
    {
        Guid Id { get; set; }
    }
}
