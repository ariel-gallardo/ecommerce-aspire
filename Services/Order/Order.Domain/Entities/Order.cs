using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;

namespace Order.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public virtual IList<OrderItem> Items { get; set; }
        public Address Address { get; set; }
    }
}
