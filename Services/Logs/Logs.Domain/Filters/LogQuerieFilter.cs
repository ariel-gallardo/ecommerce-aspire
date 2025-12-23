using Common.Domain.Entities.Base;
using Logs.Domain.Entities;
using System.Linq.Expressions;

namespace Logs.Domain.Filters
{
    public class LogQuerieFilter : QuerieFilter
    {
        public string ServiceName { get; set; }
        public string TraceId { get; set; }
        public Guid? ErrorId { get; set; }
        public Guid? Id { get; set; }
        #region Private
        private Expression<Func<Log, bool>> FindByServiceName
        {
            get => !String.IsNullOrWhiteSpace(ServiceName) ? x => x.ServiceName.Contains(ServiceName) : null;
        }
        private Expression<Func<Log, bool>> FindByTraceId
        {
            get => !String.IsNullOrWhiteSpace(TraceId) ? x => x.TraceId.Contains(TraceId) : null;
        }
        private Expression<Func<Log, bool>> FindByLogErrorId
        {
            get => ErrorId.HasValue ? x => x.ErrorId.Equals(ErrorId) : null;
        }
        private Expression<Func<Log, bool>> FindById
        {
            get => Id.HasValue ? x => x.Id.Equals(Id) : null;
        }
        #endregion
    }
}
