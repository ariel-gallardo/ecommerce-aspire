namespace Common.Infrastructure.Contracts
{
    public interface IPagedList<T> where T : class
    {
        public IList<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
