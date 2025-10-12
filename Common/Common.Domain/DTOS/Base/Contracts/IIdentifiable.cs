namespace Common.Domain.DTOS.Base.Contracts
{
    public interface IIdentifiableDTO : IEntityDTO
    {
        string Id { get; set; }
    }
}
