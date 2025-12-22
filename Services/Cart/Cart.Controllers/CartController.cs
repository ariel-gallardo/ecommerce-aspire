using Cart.Application.DTO;
using Cart.Controllers.Contracts;
using Cart.Domain.Filters.Queries;
using Common.Api.Controllers;
using Common.Infrastructure.Contracts;
using CartEntity = Cart.Domain.Entities.Cart;

namespace Cart.Controllers
{
    public class CartController : CommonController<Guid, CartEntity, CartDTO, CartDTO, CartDTO, CartQuerieFilter>, ICartController
    {
        public CartController(ICommonServices services) : base(services)
        {
        }
    }
}
