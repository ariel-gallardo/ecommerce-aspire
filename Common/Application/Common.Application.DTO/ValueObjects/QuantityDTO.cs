using Common.Infrastructure.Entities.Enums;

namespace Common.Application.DTO.ValueObjects
{
    public class QuantityDTO : Base.Entities.DTO
    {
        public decimal Value { get; set; }
        public Unit? Unit { get; set; }
    }
}
