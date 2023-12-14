using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contracts;

public interface IRepository<T> where T : class
{

    IQueryable<T> GetAll();
    Task<T> GetByIdAsync(int id);
    T GetById(int id);
    void Add(T entity, bool fillCreatedUser = true, bool fillUpdatedUser = true);
    Task AddAsync(T entity, bool fillCreatedUser = true, bool fillUpdatedUser = true);
    void Update(T entity, bool fillUpdatedUser = true);
    void Delete(T entity);
    DbSet<T> GetDbSet();
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}