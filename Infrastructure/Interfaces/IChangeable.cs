namespace Infrastructure.Interfaces;

public interface IChangeable
{
    DateTime Created { get; set; }
    DateTime Updated { get; set; }
}