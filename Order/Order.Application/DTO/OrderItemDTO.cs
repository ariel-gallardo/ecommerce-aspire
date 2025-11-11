using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Contracts.DTO.ABM;

namespace Order.Application.DTO
{
    public class OrderItemDTO : AuditableDTO, IAddDTO, IUpdateDTO
    {
        public Guid ProductId { get; set; }
        public PriceDTO Price { get; set; }
        public QuantityDTO Quantity { get; set; }
        public Guid OrderId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OrderItemDTO dTO &&
                   base.Equals(obj) &&
                   ProductId.Equals(dTO.ProductId) &&
                   EqualityComparer<PriceDTO>.Default.Equals(Price, dTO.Price) &&
                   EqualityComparer<QuantityDTO>.Default.Equals(Quantity, dTO.Quantity) &&
                   OrderId.Equals(dTO.OrderId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ProductId, Price, Quantity, OrderId);
        }
    }
}
