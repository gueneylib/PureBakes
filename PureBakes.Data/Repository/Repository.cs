namespace PureBakes.Data.Repository;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PureBakes.Data.Repository.Interface;

// Read about generic repository here: https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#create-a-generic-repository

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
}