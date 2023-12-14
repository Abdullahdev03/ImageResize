
using Infrastructure.Entities;

namespace Infrastructure.Data.Contracts;

public interface IUnitOfWork
{
    int Commit();
    Task<int> CommitAsync();

    IRepository<T> GetStandardRepo<T>() where T : class;
    
    IRepository<ApplicationUser> Users { get; }
    IRepository<FileStorage> FileStorages { get; }
}