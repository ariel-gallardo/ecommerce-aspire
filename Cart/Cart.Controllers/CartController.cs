using Cart.Application.DTO;
using Cart.Controllers.Contracts;
using Cart.Domain.Filters.Queries;
using Common.Api.Controllers;
using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using CartEntity = Cart.Domain.Entities.Cart;

namespace Cart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : CommonController<CartEntity, CartDTO, CartDTO, CartDTO, CartQuerieFilter>, ICartController
    {
        public CartController(ICommonServices services) : base(services)
        {
        }
    }
}
