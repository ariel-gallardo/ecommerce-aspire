using Common.Domain.Enums;

namespace Common.Domain.ValueObjects
{
    public class Quantity
    {
        public decimal Value { get; set; }
        public Unit Unit { get; set; }
    }
}
