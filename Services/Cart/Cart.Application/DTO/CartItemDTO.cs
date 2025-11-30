using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Contracts.DTO.ABM;

namespace Cart.Application.DTO
{
    public class CartItemDTO : AuditableDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public Guid ProductId { get; set; }
        public QuantityDTO Quantity { get; set; }
        public virtual Guid CartId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CartItemDTO dTO &&
                   base.Equals(obj) &&
                   ProductId.Equals(dTO.ProductId) &&
                   EqualityComparer<QuantityDTO>.Default.Equals(Quantity, dTO.Quantity) &&
                   CartId.Equals(dTO.CartId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ProductId, Quantity, CartId);
        }
    }
}
