using Common.Domain.Enums;
using Common.Domain.ValueObjects;

namespace Common.Application.DTO.ValueObjects
{
    public class QuantityDTO : Base.Entities.DTO
    {
        public decimal Value { get; set; }
        public string Unit { get; set; } = string.Empty;

        public Quantity AsDomain()
        {
            if (!Enum.TryParse<Unit>(Unit, out var parsedUnit))
                throw new InvalidOperationException($"Invalid unit: {Unit}");
            return new Quantity { Unit = parsedUnit, Value = Value };
        }

    }
}
