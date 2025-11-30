using Common.Domain.Entities.Base;

namespace Cart.Domain.Entities
{
    public class Cart : AuditableEntity
    {
        public virtual IList<CartItem> Items { get; set; }
    }
}
