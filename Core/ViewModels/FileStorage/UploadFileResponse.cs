namespace Core.ViewModels.FileStorage;

public class UploadFileResponse
{
    public string FileLocation { get; set; }

    public UploadFileResponse(string path)
    
    {
        FileLocation = path;
    }
}
