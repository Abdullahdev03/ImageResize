using Infrastructure.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Helpers;

public class RepositoryProvider : IRepositoryProvider
{
    public RepositoryProvider(RepositoryFactories repositoryFactories)
    {
        _repositoryFactories = repositoryFactories;
        Repositories = new Dictionary<Type, object>();
    }


    public DbContext DbContext { get; set; }

    public IRepository<T> GetRepositoryForEntityType<T>() where T : class
    {
        return GetRepository<IRepository<T>>(
            _repositoryFactories.GetRepositoryFactoryForEntityType<T>());
    }

   

    public virtual T GetRepository<T>(Func<DbContext, object> factory = null) where T : class
    {
        // Look for T dictionary cache under typeof(T).
        object repoObj;
        Repositories.TryGetValue(typeof(T), out repoObj);
        if (repoObj != null)
        {
            return (T)repoObj;
        }

        // Not found or null; make one, add to dictionary cache, and return it.
        return MakeRepository<T>(factory, DbContext);
    }


    protected Dictionary<Type, object> Repositories { get; private set; }


    protected virtual T MakeRepository<T>(Func<DbContext, object> factory, DbContext dbContext)
    {
        var f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();
        if (f == null)
        {
            throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
        }

        var repo = (T)f(dbContext);
        Repositories[typeof(T)] = repo;
        return repo;
    }


    public void SetRepository<T>(T repository)
    {
        Repositories[typeof(T)] = repository;
    }

    private readonly RepositoryFactories _repositoryFactories;
}