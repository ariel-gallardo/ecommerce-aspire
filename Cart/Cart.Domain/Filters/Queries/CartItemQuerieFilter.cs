using Cart.Domain.Entities;
using Common.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Cart.Domain.Filters.Queries
{
    public class CartItemQuerieFilter : QuerieFilter
    {
        public CartItemQuerieFilter()
        {
            ProductIds = Array.Empty<string>();
        }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public IEnumerable<string> ProductIds { get; set; }
        public string CartId { get; set; }

        #region Expressions
        private Expression<Func<CartItem, bool>>? FindById
        {
            get => !string.IsNullOrWhiteSpace(Id) ? x => x.Id.Equals(Guid.Parse(ProductId)) : null;
        }
        private Expression<Func<CartItem, bool>>? FindByProductId
        {
            get => !string.IsNullOrWhiteSpace(ProductId) ? x =>  x.ProductId.Equals(Guid.Parse(ProductId)) : null;
        }
        private Expression<Func<CartItem, bool>>? FindByProductIds
        {
            get => ProductIds.Any() ? x => ProductIds.Any(y => x.ProductId.Equals(Guid.Parse(y))) : null;
        }
        private Expression<Func<CartItem, bool>>? FindByCartId
        {
            get => !string.IsNullOrWhiteSpace(CartId) ? x => x.CartId.Equals(Guid.Parse(CartId)) : null;
        }
        #endregion
    }
}
