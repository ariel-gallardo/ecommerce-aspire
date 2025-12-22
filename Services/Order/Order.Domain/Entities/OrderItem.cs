using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;

namespace Order.Domain.Entities
{
    public class OrderItem : AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Price Price { get; set; }
        public Quantity Quantity { get; set; }
        public virtual Order Order { get; set; }
        public Guid OrderId { get;set; }
    }
}
