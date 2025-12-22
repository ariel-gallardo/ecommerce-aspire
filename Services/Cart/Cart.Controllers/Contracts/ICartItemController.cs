using Cart.Application.DTO;
using Cart.Domain.Entities;
using Cart.Domain.Filters.Queries;
using Common.Infrastructure.Contracts;

namespace Cart.Controllers.Contracts
{
    public interface ICartItemController : ICommonController<Guid, CartItem, CartItemDTO, CartItemDTO, CartItemDTO, CartItemQuerieFilter>
    {
    }
}
