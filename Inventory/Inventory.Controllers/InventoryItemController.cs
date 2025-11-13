using Common.Api.Controllers;
using Common.Contracts;
using Inventory.Application.DTO;
using Inventory.Controllers.Contracts;
using Inventory.Domain.Entities;
using Inventory.Domain.Filters.Querie;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryItemController : CommonController<InventoryItem, InventoryItemDTO, InventoryItemDTO, InventoryItemDTO, InventoryItemDTOQuerieFilter>, IInventoryItemController
    {
        public InventoryItemController(ICommonServices services) : base(services)
        {
        }
    }
}
