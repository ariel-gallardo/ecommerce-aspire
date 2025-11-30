using Common.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Domain.Filters.Querie
{
    public class InventoryItemQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public string ProductId { get; set; }
        [FromQuery]
        public string Unit { get; set; }
        /*[FromQuery]
        public QuantityDTO? Quantity { get; set; }
        [FromQuery]
        public QuantityDTO? QuantityAlert { get; set; }*/
        [FromQuery]
        public string OperatorQuantity { get; set; }
        [FromQuery]
        public string OperatorQuantityAlert { get; set; }

        #region Expressions
        /*
        private Expression<Func<InventoryItem, bool>> FindByProductId
        {
            get => x => !string.IsNullOrEmpty(ProductId) && x.ProductId == Guid.Parse(ProductId);
        }

        private Expression<Func<InventoryItem, bool>> FindByUnit
        {
            get => x => !string.IsNullOrEmpty(Unit) && x.Unit == Enum.Parse<Unit>(Unit);
        }
        private Expression<Func<InventoryItem, bool>> FindByQuantity
        {
            get => InventoryExpressions.BuildQuantityExpression<InventoryItem>(OperatorQuantity, Quantity, x => x.Quantity);
        }

        private Expression<Func<InventoryItem, bool>> FindByQuantityAlert
        {
            get => InventoryExpressions.BuildQuantityExpression<InventoryItem>(OperatorQuantityAlert, QuantityAlert, x => x.QuantityAlert);
        }*/
        #endregion
    }
}
