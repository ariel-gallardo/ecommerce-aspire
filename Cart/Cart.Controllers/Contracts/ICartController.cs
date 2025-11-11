using Cart.Application.DTO;
using Cart.Domain.Filters.Queries;
using Common.Contracts;
using CartEntity = Cart.Domain.Entities.Cart;

namespace Cart.Controllers.Contracts
{
    public interface ICartController : ICommonController<CartEntity,CartDTO,CartDTO,CartDTO,CartQuerieFilter>
    {
    }
}
