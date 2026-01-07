using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Infrastructure.Contracts;

namespace Security.Application.DTO
{
    public class PersonaDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IReadDTO
    {
        public string Name { get; set; }
        public string Lastname { get; set; }

        public AddressDTO Address { get; set; }
    }
}
