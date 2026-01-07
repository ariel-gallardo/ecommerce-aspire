namespace Common.Infrastructure.Contracts
{
    public interface IIdentifiableDTO : IEntityDTO
    {
        ulong Id { get; set; }
    }
}
