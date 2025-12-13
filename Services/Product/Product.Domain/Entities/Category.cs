using Common.Domain.Entities.Base;

namespace Product.Domain.Entities
{
    public class Category : AuditableGuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set;} = new List<Category>();
        public virtual IList<Product> Products { get; set; } = new List<Product>();
        public Guid? ParentId { get; set; }
    }
}
