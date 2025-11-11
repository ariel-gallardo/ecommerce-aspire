using Common.Domain.Entities.Base;
using Common.Domain.Enums;
using Common.Domain.ValueObjects;

namespace Inventory.Domain
{
    public class InventoryItem : AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Quantity Quantity { get; set; }
        public Quantity QuantityAlert { get; set; }
        public Unit Unit { get; set; }
    }
}
