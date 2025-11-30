using Common.Domain.Entities.Base;

namespace Product.Domain.Entities
{
    public class Category : AuditableGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Category Parent { get; set; }
        public virtual IList<Category> Children { get; set; }
        public virtual IList<Product> Products { get; set; }
        public Guid? ParentId { get; set; }
    }
}
