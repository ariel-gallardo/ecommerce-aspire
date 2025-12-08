using Common.Contracts.DTO.Base;

namespace Common.Application.DTO.Base.Entities
{
    public class IdentifiableGuidDTO : DTO, IIdentifiableGuidDTO
    {
        public Guid Id { get; set; }
    }
}
