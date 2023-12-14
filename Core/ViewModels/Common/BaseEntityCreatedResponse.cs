namespace Core.ViewModels.Common;

public class BaseEntityCreatedResponse
{
    public int Id { get; set; }

    public BaseEntityCreatedResponse(int id)
    {
        Id = id;
    }
}
