using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Infrastructure.Contracts;

namespace Cart.Application.DTO
{
    public class CartItemDTO : AuditableGuidDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public Guid ProductId { get; set; }
        public QuantityDTO Quantity { get; set; }
        public virtual Guid CartId { get; set; }
    }
}
