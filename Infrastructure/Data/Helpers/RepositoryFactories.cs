using Infrastructure.Data.Concrete;
using Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Helpers
{
    public class RepositoryFactories
    {
        private readonly UserProvider _userProvider;
        
        private IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {
                //{typeof(IArticleRepository), dbContext => new ArticleRepository(dbContext)},
                //{typeof(IUrlRepository), dbContext => new UrlRepository(dbContext)},
            };
        }
        
        public RepositoryFactories(UserProvider userProvider)
        {
            _userProvider = userProvider;
            _repositoryFactories = GetFactories();
        }

       
        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }

      
        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        
        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

      
        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new EfRepository<T>(dbContext, _userProvider);
        }

        
        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;
    }
}