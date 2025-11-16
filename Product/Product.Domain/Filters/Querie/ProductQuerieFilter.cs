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
        private Expression<Func<Entities.Product, bool>>? FindByName
        {
            get => !string.IsNullOrEmpty(Name) ? x => EF.Functions.Like(x.Name, $"%{Name}%") : null;
        }

        private Expression<Func<Entities.Product, bool>>? FindByDescription
        {
            get => !string.IsNullOrEmpty(Description) ? x => EF.Functions.Like(x.Description, $"%{Description}%") : null;
        }

        private Expression<Func<Entities.Product, bool>>? FindByCategory
        {
            get => !string.IsNullOrEmpty(Category) ? x => EF.Functions.Like(x.Category.Name, $"%{Category}%") : null;
        }
        private Expression<Func<Entities.Product, bool>>? FindByCategoryId
        {
            get => CategoryId.HasValue ? x => x.CategoryId == CategoryId : null;
        }
        #endregion
    }
}
