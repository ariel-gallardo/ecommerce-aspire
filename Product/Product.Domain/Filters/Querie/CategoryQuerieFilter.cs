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
        private Expression<Func<Entities.Category, bool>> FindByName
        {
            get => x => !string.IsNullOrEmpty(Name) && EF.Functions.Like(x.Name,$"%{Name}%");
        }
        private Expression<Func<Entities.Category, bool>> FindByDescription
        {
            get => x => !string.IsNullOrEmpty(Description) && EF.Functions.Like(x.Description,$"%{Description}%");
        }
        #endregion
    }
}
