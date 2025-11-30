using Common.Contracts;
using Inventory.Application.DTO;
using Inventory.Domain.Entities;
using Inventory.Domain.Filters.Querie;

namespace Inventory.Controllers.Contracts
{
    public interface IInventoryItemController : ICommonController<InventoryItem, InventoryItemDTO, InventoryItemDTO, InventoryItemDTO, InventoryItemDTOQuerieFilter>
    {
    }
}
