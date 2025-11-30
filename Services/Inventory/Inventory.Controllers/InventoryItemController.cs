using Common.Api.Controllers;
using Common.Contracts;
using Inventory.Application.DTO;
using Inventory.Controllers.Contracts;
using Inventory.Domain.Entities;
using Inventory.Domain.Filters.Querie;

namespace Inventory.Controllers
{
    public class InventoryItemController : CommonController<InventoryItem, InventoryItemDTO, InventoryItemDTO, InventoryItemDTO, InventoryItemQuerieFilter>, IInventoryItemController
    {
        public InventoryItemController(ICommonServices services) : base(services)
        {
        }
    }
}
