namespace Common.Infrastructure.Contracts
{
    public interface IQuerieFilter
    {
        bool? TakeAll { get; set; }
        string OrderBy { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
