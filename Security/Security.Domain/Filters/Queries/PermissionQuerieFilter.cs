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
        private Expression<Func<Permission,bool>>? FindByControllerName
        {
            get => !string.IsNullOrWhiteSpace(Controller) ? x => x.Controller == Controller : null;
        }
        private Expression<Func<Permission, bool>>? FindByAction
        {
            get => !string.IsNullOrWhiteSpace(Action) ? x => x.Action == Action : null;
        }
        private Expression<Func<Permission, bool>>? FindByUrl
        {
            get => !string.IsNullOrWhiteSpace(Url) ? x => x.Url == Url : null;
        }
        #endregion
    }
}
