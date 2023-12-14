namespace Core.ViewModel;
public class FilterDto
{
    public string Value { get; set; }
    public string Property { get; set; }
    public FilterDto(string value, string property)
    {
        Value = value;
        Property = property;
    }
}
