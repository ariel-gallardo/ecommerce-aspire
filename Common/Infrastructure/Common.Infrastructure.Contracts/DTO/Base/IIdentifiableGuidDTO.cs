namespace Common.Infrastructure.Contracts
{
    public interface IIdentifiableGuidDTO : IEntityDTO
    {
        Guid Id { get; set; }
    }
}
