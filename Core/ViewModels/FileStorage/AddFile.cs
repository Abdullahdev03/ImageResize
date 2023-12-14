using Microsoft.AspNetCore.Http;

namespace Core.ViewModels.FileStorage;

public class AddFile
{
    public IFormFile File { get; set; }
}