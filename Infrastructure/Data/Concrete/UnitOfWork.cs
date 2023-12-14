using Infrastructure.Data.Contracts;
using Infrastructure.Entities;
using Infrastructure.Providers;

namespace Infrastructure.Data.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserProvider _userProvider;
    private DataContext DataContext { get; set; }
    public IRepositoryProvider RepositoryProvider { get; set; }


    public UnitOfWork(DataContext dataContext, IRepositoryProvider repositoryProvider)
    { 
        DataContext = dataContext;
        repositoryProvider.DbContext = DataContext;
        RepositoryProvider = repositoryProvider;
        
    }
    
    public IRepository<ApplicationUser> Users { get => GetStandardRepo<ApplicationUser>(); }
    public IRepository<FileStorage> FileStorages { get => GetStandardRepo<FileStorage>(); }

    public IRepository<T> GetStandardRepo<T>() where T : class
    {
        return RepositoryProvider.GetRepositoryForEntityType<T>();
    }
    
    public int Commit()
    {
        return DataContext.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await DataContext.SaveChangesAsync();
    }
}