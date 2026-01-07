using Common.Infrastructure.Entities.Enums;

namespace Common.Domain.ValueObjects
{
    public class Price
    {
        public decimal Value { get; set; }
        public Unit Unit { get; set; }
    }
}
