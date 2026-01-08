using Common.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Product.Domain.Filters.Querie
{
    public class ProductQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public bool? WithPrice { get; set; }
        [FromQuery]
        public string Name { get; set; }
        [FromQuery]
        public string Description { get; set; }
        [FromQuery]
        public string Category { get; set; }
        [FromQuery]
        public Guid? CategoryId { get; set; }
        [FromQuery]
        public IList<Guid>? Ids { get; set; }
        [FromQuery]
        public decimal? PriceFrom { get; set; }
        [FromQuery]
        public decimal? PriceTo { get; set; }

        #region Expressions
        private Expression<Func<Entities.Product, bool>>? FindWithPrice
        {
            get => WithPrice.HasValue ? x => x.Price != null && x.Price.Value > 0.0m : null;
        }

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
        private Expression<Func<Entities.Product, bool>>? FindByIds
        {
            get => Ids != null && Ids.Any() ? x => Ids.Contains(x.Id) : null;
        }

        private Expression<Func<Entities.Product, bool>>? FindByPriceGreaterOrEquals
        {
            get => PriceFrom.HasValue ? x => x.Price != null && x.Price.Value >= PriceFrom.Value : null;
        }

        private Expression<Func<Entities.Product, bool>>? FindByPriceLowerOrEquals
        {
            get => PriceTo.HasValue ? x => x.Price != null && x.Price.Value <= PriceTo.Value : null;
        }
        #endregion
    }
}
