using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;
using Payment.Domain.Enums;

namespace Payment.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Price Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
