using Core.Enums;

namespace Core.ViewModel.PagingAndFiltration;

public class BaseListRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? SortProperty { get; set; }
    public SortDirection SortDir { get; set; }

    public BaseListRequest()
    {
        Page = PagingOptions.DefaultPage;
        PageSize = PagingOptions.DefaultPageSize;

    }

    internal PagingOptions PagingOptions => new(Page, PageSize);
    internal SortRule SortRule => new SortRule(SortProperty, SortDir);
}

