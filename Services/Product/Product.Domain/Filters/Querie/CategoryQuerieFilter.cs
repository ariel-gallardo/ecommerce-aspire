using Common.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace Product.Domain.Filters.Querie
{
    public class CategoryQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public string Name { get; set; }
        [FromQuery]
        public string Description { get; set; }

        #region Expressions
        private Expression<Func<Entities.Category, bool>>? FindByName
        {
            get => !string.IsNullOrEmpty(Name) ? x => EF.Functions.Like(x.Name,$"%{Name}%") : null;
        }
        private Expression<Func<Entities.Category, bool>>? FindByDescription
        {
            get =>!string.IsNullOrEmpty(Description) ? x => EF.Functions.Like(x.Description,$"%{Description}%") : null;
        }
        #endregion
    }
}
