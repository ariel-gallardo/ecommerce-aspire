using Common.Application.DTO.Base.Entities;
using Common.Application.DTO.ValueObjects;
using Common.Contracts.DTO.ABM;

namespace Inventory.Application.DTO
{
    public class InventoryItemDTO : AuditableDTO, IAddDTO, IUpdateDTO
    {
        public Guid ProductId { get; set; }
        public QuantityDTO Quantity { get; set; }
        public QuantityDTO QuantityAlert { get; set; }
        public string Unit { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InventoryItemDTO dTO &&
                   base.Equals(obj) &&
                   ProductId.Equals(dTO.ProductId) &&
                   EqualityComparer<QuantityDTO>.Default.Equals(Quantity, dTO.Quantity) &&
                   EqualityComparer<QuantityDTO>.Default.Equals(QuantityAlert, dTO.QuantityAlert) &&
                   Unit == dTO.Unit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ProductId, Quantity, QuantityAlert, Unit);
        }
    }
}
