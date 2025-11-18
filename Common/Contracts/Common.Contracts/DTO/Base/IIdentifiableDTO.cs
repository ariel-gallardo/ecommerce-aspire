namespace Common.Contracts.DTO.Base
{
    public interface IIdentifiableDTO : IEntityDTO
    {
        long Id { get; set; }
    }
}
