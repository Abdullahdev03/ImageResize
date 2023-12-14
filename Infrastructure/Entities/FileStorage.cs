using Infrastructure.Entities.BaseObjects;

namespace Infrastructure.Entities;

public class FileStorage : BaseEntity
{
    public int Id { get; set; } 
    public string FileName { get; set; }
    public string FileLocation { get; set; }
   
}