using Common.Infrastructure.Contracts;

namespace Common.Infrastructure.Entities
{
    public class PagedList<T> : IPagedList<T> where T : class
    {
        public PagedList()
        {
            CurrentPage = 0;
            TotalPages = 0;
            PageSize = 0;
            TotalCount = 0;
            Items = new List<T>();
        }
        public PagedList(IList<T> items, int count, int pageNumber, int pageSize, bool takeAll = false)
        {
            CurrentPage = takeAll ? 1 : pageNumber;
            TotalPages = takeAll ? 1 : (int)Math.Ceiling(count / (double)pageSize);
            PageSize = takeAll ? count : pageSize;
            TotalCount = count;
            Items = items;
        }
        public IList<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
