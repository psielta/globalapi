using X.PagedList;

namespace GlobalErpData.Dto
{
    public class PagedResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }

        public PagedResponse(IPagedList<T> pagedList)
        {
            CurrentPage = pagedList.PageNumber;
            TotalPages = pagedList.PageCount;
            PageSize = pagedList.PageSize;
            TotalCount = pagedList.TotalItemCount;
            Items = pagedList.ToList();
        }
    }

}
