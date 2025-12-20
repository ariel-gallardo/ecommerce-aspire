using Common.Contracts;

namespace Common.Infrastructure.Entities
{
    public class PagedList<T> : List<T>, IPagedList<T> where T : class
    {
        public PagedList()
        {
            CurrentPage = 0;
            TotalPages = 0;
            PageSize = 0;
            TotalCount = 0;
        }
        public PagedList(IList<T> items, int count, int pageNumber, int pageSize, bool takeAll = false)
        {
            CurrentPage = takeAll ? 1 : pageNumber;
            TotalPages = takeAll ? 1 : (int)Math.Ceiling(count / (double)pageSize);
            PageSize = takeAll ? count : pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
