
namespace Core.ViewModel.PagingAndFiltration;

public class BaseFilterableListRequest : BaseListRequest
{
    public string? Value { get; set; }
    public string? Property { get; set; }
    public BaseFilterableListRequest() : base()
    {

    }
    internal FilterDto FilterOptions => new FilterDto(Value, Property);
}
