using Cart.Application.DTO;
using Cart.Controllers.Contracts;
using Cart.Domain.Entities;
using Cart.Domain.Filters.Queries;
using Common.Api.Controllers;
using Common.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : CommonController<Guid, CartItem, CartItemDTO, CartItemDTO, CartItemDTO, CartItemQuerieFilter>, ICartItemController
    {
        public CartItemController(ICommonServices services) : base(services)
        {
        }
    }
}
