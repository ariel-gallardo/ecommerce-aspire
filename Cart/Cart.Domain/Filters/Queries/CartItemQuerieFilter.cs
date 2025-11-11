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
        private Expression<Func<CartItem, bool>> FindById
        {
            get => x => !string.IsNullOrWhiteSpace(Id) ? x.Id.Equals(Guid.Parse(ProductId)) : true;
        }
        private Expression<Func<CartItem, bool>> FindByProductId
        {
            get => x => !string.IsNullOrWhiteSpace(ProductId) ?  x.ProductId.Equals(Guid.Parse(ProductId)) : true;
        }
        private Expression<Func<CartItem, bool>> FindByProductIds
        {
            get => x => ProductIds.Any() ? ProductIds.Any(y => x.ProductId.Equals(Guid.Parse(y))) : true;
        }
        private Expression<Func<CartItem, bool>> FindByCartId
        {
            get => x => !string.IsNullOrWhiteSpace(CartId) ? x.CartId.Equals(Guid.Parse(CartId)) : true;
        }
        #endregion
    }
}
