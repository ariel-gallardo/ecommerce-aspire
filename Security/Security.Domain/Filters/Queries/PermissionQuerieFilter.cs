using Common.Domain.Entities.Base;
using Security.Domain.Entities;
using System.Linq.Expressions;

namespace Security.Domain.Filters.Queries
{
    public class PermissionQuerieFilter : QuerieFilter
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }

        #region Expressions
        private Expression<Func<Permission,bool>> FindByControllerName
        {
            get => x => !string.IsNullOrWhiteSpace(Controller) ? x.Controller == Controller : false;
        }
        private Expression<Func<Permission, bool>> FindByAction
        {
            get => x => !string.IsNullOrWhiteSpace(Action) ? x.Action == Action : false;
        }
        private Expression<Func<Permission, bool>> FindByUrl
        {
            get => x => !string.IsNullOrWhiteSpace(Url) ? x.Url == Url : false;
        }
        #endregion
    }
}
