using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Contracts.DTO.ABM;

namespace Payment.Application.DTO
{
    public class PaymentDTO : AuditableDTO, IAddDTO, IUpdateDTO
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public PriceDTO Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PaymentDTO dTO &&
                   base.Equals(obj) &&
                   OrderId.Equals(dTO.OrderId) &&
                   UserId.Equals(dTO.UserId) &&
                   EqualityComparer<PriceDTO>.Default.Equals(Amount, dTO.Amount) &&
                   PaymentMethod == dTO.PaymentMethod &&
                   PaymentStatus == dTO.PaymentStatus;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), OrderId, UserId, Amount, PaymentMethod, PaymentStatus);
        }
    }
}
