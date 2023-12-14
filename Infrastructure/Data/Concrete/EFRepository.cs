using System.Linq.Expressions;
using Infrastructure.Data.Contracts;
using Infrastructure.Interfaces;
using Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data.Concrete;

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly UserProvider _userProvider;

    public EfRepository(DbContext dbContext, UserProvider userProvider)
    {
        _userProvider = userProvider;
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = DbContext.Set<T>();
    }

    protected DbContext DbContext { get; set; }

    protected DbSet<T> DbSet { get; set; }


    public T GetById(int id)
    {
        return DbContext.Set<T>().Find(id);
    }


    public async Task<T> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Add(T entity, bool fillCreatedUser = true, bool fillUpdatedUser = true)
    {
        if (entity is IChangeable changeable)
        {
            changeable.Created = DateTime.UtcNow;
            changeable.Updated = DateTime.UtcNow;
        }
        if (entity is IDeleteable deletable)
        {
            deletable.IsDeleted = false;
        }
        if (entity is ICreateableByUser creatableByUser && fillCreatedUser)
            creatableByUser.CreatedUserId = _userProvider.CurrentUser.Id;

        if (entity is IUpdateableByUser updateableByUser && fillUpdatedUser)
            updateableByUser.UpdatedUserId = _userProvider.CurrentUser.Id;

        EntityEntry dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Detached)
        {
            dbEntityEntry.State = EntityState.Added;
        }
        else
        {
            DbSet.Add(entity);
        }
    }

    public async Task AddAsync(T entity, bool fillCreatedUser = true, bool fillUpdatedUser = true)
    {
        if (entity is IChangeable changeable)
        {
            changeable.Created = DateTime.UtcNow;
            changeable.Updated = DateTime.UtcNow;
        }

        if (entity is IDeleteable deletable)
        {
            deletable.IsDeleted = false;
        }
        if (entity is ICreateableByUser creatableByUser && fillCreatedUser)
            creatableByUser.CreatedUserId = _userProvider.CurrentUser.Id;

        if (entity is IUpdateableByUser updateableByUser && fillUpdatedUser)
            updateableByUser.UpdatedUserId = _userProvider.CurrentUser.Id;

        EntityEntry dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Detached)
        {
            dbEntityEntry.State = EntityState.Added;
        }
        else
        {
            await DbSet.AddAsync(entity);
        }
    }

    public virtual void Update(T entity, bool fillUpdatedUser = true)
    {
        EntityEntry dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        dbEntityEntry.State = EntityState.Modified;

        if (entity is ICreateableByUser creatableByUser)
            DbContext.Entry(creatableByUser).Property(x => x.CreatedUserId).IsModified = false;

        if (entity is IUpdateableByUser updateableByUser && fillUpdatedUser)
            updateableByUser.UpdatedUserId = _userProvider.CurrentUser.Id;


        if (entity is IChangeable changeable)
        {
            changeable.Updated = DateTime.UtcNow; //GetDateTime();
            DbContext.Entry(changeable).Property(x => x.Created).IsModified = false;
        }
    }
    
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }

    public DbSet<T> GetDbSet()
    {
        return DbSet;
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }


    IQueryable<T> IRepository<T>.GetAll()
    {
        var query = DbSet.AsQueryable<T>();
        return query;
    }


    public virtual void Delete(int id)
    {
        var entity = GetById(id);
        if (entity == null) return; // not found; assume already deleted.
        Delete(entity);
    }

    public virtual void Delete(T entity)
    {
        if (entity is IUpdateableByUser updateableByUser)
            updateableByUser.UpdatedUserId = _userProvider.CurrentUser.Id;
        if (entity is IChangeable changeable)
        {
            changeable.Updated = DateTime.Now; //GetDateTime();
            DbContext.Entry(changeable).Property(x => x.Created).IsModified = false;
        }
        

        if (entity is IDeleteable deletable)
        {
            deletable.IsDeleted = true;
            if (entity is IActiveable activeable)
            {
                activeable.IsPublish = false;
            }

            Update(entity);
        }
        else
        {
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }
    }
}
