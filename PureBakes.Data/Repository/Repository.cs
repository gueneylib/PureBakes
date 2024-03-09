namespace PureBakes.Data.Repository;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PureBakes.Data.Repository.Interface;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly PureBakesDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(PureBakesDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public IEnumerable<T> GetAll(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>,IOrderedQueryable<T>>? orderBy,
        string includeProperties)
    {
        var query = _dbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }

        return query.ToList();
    }

    public T? Get(Expression<Func<T, bool>>? filter,
        string includeProperties,
        bool tracked)
    {
        IQueryable<T> query;
        if (tracked) {
            query= _dbSet;

        }
        else {
            query = _dbSet.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault();
    }

    public T? Get(
        object id,
        string includeProperties,
        bool tracked)
    {
        if (!tracked)
            _dbSet.AsNoTracking();

        return _dbSet.Find(id);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Remove(T entity)
    {

        // As far as i understood, below code is for removing the entity without calling "SaveChanges()"
        // if (_dbContext.Entry(entity).State == EntityState.Detached)
        // {
        //     _dbSet.Attach(entity);
        // }
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}