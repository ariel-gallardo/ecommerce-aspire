namespace Common.Contracts.DTO.Base
{
    public interface IIdentifiableDTO : IEntityDTO
    {
        string Id { get; set; }
    }
}
