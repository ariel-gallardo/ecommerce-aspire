using Common.Infrastructure.Contracts;

namespace Common.Application.DTO.Base.Entities
{
    public class IdentifiableDTO : DTO, IIdentifiableDTO
    {
        public ulong Id { get; set; }
    }
}
