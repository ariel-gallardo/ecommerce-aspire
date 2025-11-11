using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;

namespace Product.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public virtual Price Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
