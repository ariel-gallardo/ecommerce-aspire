using Common.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            get => x => !string.IsNullOrEmpty(Name) && EF.Functions.Like(x.Name, $"%{Name}%");
        }

        private Expression<Func<Entities.Product, bool>> FindByDescription
        {
            get => x => !string.IsNullOrEmpty(Description) && EF.Functions.Like(x.Description, $"%{Description}%");
        }

        private Expression<Func<Entities.Product, bool>> FindByCategory
        {
            get => x => !string.IsNullOrEmpty(Category) && EF.Functions.Like(x.Category.Name, $"%{Category}%");
        }
        private Expression<Func<Entities.Product, bool>> FindByCategoryId
        {
            get => x => CategoryId.HasValue && x.CategoryId == CategoryId;
        }
        #endregion
    }
}
