using Common.Infrastructure.Contracts;
using Product.Domain.Entities;

namespace Product.Domain.Modifier
{
    public class CategoryQueryModifier : IQueryModifier<Category>
    {
        public IQueryable<Category> Apply(IQueryable<Category> query)
        {
            return query;
        }
    }

}
