namespace Common.Contracts.DTOS
{
    public interface IIdentifiableDTO : IEntityDTO
    {
        string Id { get; set; }
    }
}
