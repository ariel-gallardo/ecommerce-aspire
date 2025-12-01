namespace Common.Contracts.DTO.Base
{
    public interface IIdentifiableGuidDTO : IEntityDTO
    {
        Guid Id { get; set; }
    }
}
