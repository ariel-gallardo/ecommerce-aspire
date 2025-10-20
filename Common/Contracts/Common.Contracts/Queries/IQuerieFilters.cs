namespace Common.Contracts.Queries
{
    public interface IQuerieFilter
    {
        string OrderBy { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
