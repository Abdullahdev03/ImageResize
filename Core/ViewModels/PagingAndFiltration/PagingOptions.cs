namespace Core.ViewModel.PagingAndFiltration;

public class PagingOptions
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public PagingOptions()
    {
        Page = DefaultPage;
        PageSize = DefaultPageSize;
    }

    public PagingOptions(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public static int DefaultPage = 1;
    public static int DefaultPageSize = 20;
}
