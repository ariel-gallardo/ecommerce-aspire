namespace Common.Contracts.DTO.Base
{
    public interface IIdentifiableDTO : IEntityDTO
    {
        ulong Id { get; set; }
    }
}
