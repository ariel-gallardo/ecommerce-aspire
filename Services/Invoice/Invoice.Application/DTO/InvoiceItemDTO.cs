using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Infrastructure.Contracts;

namespace Invoice.Application.DTO
{
    public class InvoiceItemDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO
    {
        public Guid ProductId { get; set; }
        public PriceDTO Price { get; set; }
        public QuantityDTO Quantity { get; set; }
        public Guid InvoiceId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InvoiceItemDTO dTO &&
                   base.Equals(obj) &&
                   ProductId.Equals(dTO.ProductId) &&
                   EqualityComparer<PriceDTO>.Default.Equals(Price, dTO.Price) &&
                   EqualityComparer<QuantityDTO>.Default.Equals(Quantity, dTO.Quantity) &&
                   InvoiceId.Equals(dTO.InvoiceId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ProductId, Price, Quantity, InvoiceId);
        }
    }
}
