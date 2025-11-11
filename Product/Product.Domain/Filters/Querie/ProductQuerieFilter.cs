using Common.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Product.Domain.Filters.Querie
{
    public class ProductQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public string Name { get; set; }
        [FromQuery]
        public string Description { get; set; }
        [FromQuery]
        public string Category { get; set; }
        [FromQuery]
        public Guid? CategoryId { get; set; }

        #region Expressions
        private Expression<Func<Entities.Product, bool>> FindByName
        {
            get => x => !string.IsNullOrEmpty(Name) && x.Name.Contains(Description, StringComparison.InvariantCultureIgnoreCase);
        }
        private Expression<Func<Entities.Product, bool>> FindByDescription
        {
            get => x => !string.IsNullOrEmpty(Description) && x.Description.Contains(Description,StringComparison.InvariantCultureIgnoreCase);
        }
        private Expression<Func<Entities.Product, bool>> FindByCategory
        {
            get => x => !string.IsNullOrEmpty(Category) && x.Category.Name.Contains(Category, StringComparison.InvariantCultureIgnoreCase);
        }
        private Expression<Func<Entities.Product, bool>> FindByCategoryId
        {
            get => x => CategoryId.HasValue && x.CategoryId == CategoryId;
        }
        #endregion
    }
}
