using Common.Domain.Entities.Base;
using Logs.Domain.Entities;
using System.Linq.Expressions;

namespace Logs.Domain.Filters
{
    public class LogErrorQuerieFilter : QuerieFilter
    {
        public string ExceptionType { get; set; }
        public Guid? LogId { get; set; }
        public Guid? Id { get; set; }
        
        #region Private
        private Expression<Func<LogError,bool>> FindByExceptionType
        {
            get => !string.IsNullOrWhiteSpace(ExceptionType) ? x => x.ExceptionType.Contains(ExceptionType) : null;
        }
        private Expression<Func<LogError, bool>> FindByLogId
        {
            get => LogId.HasValue ? x => x.Log.Id.Equals(LogId) : null;
        }
        private Expression<Func<LogError, bool>> FindById
        {
            get => Id.HasValue ? x => x.Id.Equals(Id) : null;
        }
        #endregion
    }
}
