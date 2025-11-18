using Common.Domain.Enums;

namespace Common.Application.DTO.ValueObjects
{
    public class PriceDTO : Base.Entities.DTO
    {
        public decimal Value { get; set; }
        public Unit? Unit { get; set; }
    }
}
