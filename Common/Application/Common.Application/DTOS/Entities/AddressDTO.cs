using Common.Domain.DTOS.Base.Entities;

namespace Common.Application.DTOS.Entities
{
    public class AddressDTO : DTO
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public string Neighborhood { get; set; }
        public string Description { get; set; }
    }
}
