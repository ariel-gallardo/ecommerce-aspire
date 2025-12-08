using Common.Domain.Entities.Base;
using System.Linq.Expressions;
using CartEntity = Cart.Domain.Entities.Cart;

namespace Cart.Domain.Filters.Queries
{
    public class CartQuerieFilter : QuerieFilter
    {
        public CartQuerieFilter()
        {
            ProductIds = Array.Empty<string>();
        }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public ulong? UserId { get; set; }
        public IEnumerable<string> ProductIds { get; set; }
        #region Expressions
        private Expression<Func<CartEntity, bool>>? FindById
        {
            get => !string.IsNullOrWhiteSpace(Id) ? x=> x.Id.Equals(Guid.Parse(ProductId)) : null;
        }
        private Expression<Func<CartEntity,bool>>? FindByProductId
        {
            get =>  !string.IsNullOrWhiteSpace(ProductId) ? x=> x.Items.Any(x => x.ProductId.Equals(Guid.Parse(ProductId))) : null;
        }
        private Expression<Func<CartEntity, bool>>? FindByProductIds
        {
            get => ProductIds.Any() ? x => ProductIds.Any(y => x.Items.Any(z => z.ProductId.Equals(Guid.Parse(y)))) : null;
        }
        private Expression<Func<CartEntity, bool>>? FindByUserId
        {
            get => UserId.HasValue ? x => x.CreatedById == UserId : null;
        }
        #endregion
    }
}
