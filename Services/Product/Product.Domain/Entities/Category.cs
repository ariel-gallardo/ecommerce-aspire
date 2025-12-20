using Common.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Product.Domain.Entities
{
    public class Category : AuditableGuidEntity
    {
        public Category(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }
        private ICollection<Category> _children = new List<Category>();
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get => LazyLoader.Load(this, ref _children) ; set => _children = value; }
        public virtual IList<Product> Products { get; set; }
        public Guid? ParentId { get; set; }
    }
}
