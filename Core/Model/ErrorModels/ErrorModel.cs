using Core.Enums;
using Core.Staff;

namespace Core.Model.ErrorModels;

public class ErrorModel
{
    public string Name { get; set; }
    public ErrorCode Code { get; set; }
    public String Description { get; set; }

    public ErrorModel()
    {

    }

    public ErrorModel(string description)
    {
        Description = description;
    }
    public ErrorModel(string code, string description)
    {
        Code = Enum.Parse<ErrorCode>(code);
        Description = description;
    }
    public ErrorModel(ErrorCode code)
    {
        Code = code;
        Description = code.ToStringX();
    }
    public ErrorModel(ErrorCode code, string fieldName)
    {
        Code = code;
        Description = code.ToStringX();
        Name = fieldName;
    }
}
