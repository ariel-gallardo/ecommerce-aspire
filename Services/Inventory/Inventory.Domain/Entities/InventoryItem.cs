using Common.Domain.Entities.Base;
using Common.Domain.Enums;
using Common.Domain.ValueObjects;

namespace Inventory.Domain.Entities
{
    public class InventoryItem : AuditableGuidEntity
    {
        public Guid ProductId { get; set; }
        public Quantity Quantity { get; set; }
        public Quantity QuantityAlert { get; set; }
        public Unit Unit { get; set; }
    }
}
