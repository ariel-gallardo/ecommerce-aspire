using Common.Infrastructure.Contracts;

namespace Common.Application.DTO.Base.Entities
{
    public class IdentifiableGuidDTO : DTO, IIdentifiableGuidDTO
    {
        public Guid Id { get; set; }
    }
}
