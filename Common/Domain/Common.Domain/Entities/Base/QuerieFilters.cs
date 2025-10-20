using Common.Contracts.Queries;

namespace Common.Domain.Entities.Base
{
    public class QuerieFilter : IQuerieFilter
    {
        public string OrderBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
