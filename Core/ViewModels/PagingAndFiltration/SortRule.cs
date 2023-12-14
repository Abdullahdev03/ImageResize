

using Core.Enums;

namespace Core.ViewModel.PagingAndFiltration;

public class SortRule
{
    public string SortProperty { get; set; }

    public SortDirection SortDir { get; set; }

    public SortRule(string sortProperty, SortDirection sortDir)
    {
        SortProperty = sortProperty;
        SortDir = sortDir;
    }
}
