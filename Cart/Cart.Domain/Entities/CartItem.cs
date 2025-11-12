using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;

namespace Cart.Domain.Entities
{
    public class CartItem : AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Quantity Quantity { get; set; }
        public virtual Cart Cart {get;set;}
        public Guid CartId {get;set; }
    }
}
