namespace Core.Staff;

public class PagedList<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Page { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public PagedList(IEnumerable<T> items, int count, int page, int pageSize)
    {
        Page = page;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        PageSize = pageSize;
        Items = items;
    }
}
