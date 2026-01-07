using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;
using Common.Infrastructure.Entities.Enums;

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
