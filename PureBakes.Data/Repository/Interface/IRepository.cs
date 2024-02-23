namespace PureBakes.Data.Repository.Interface;

using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "");

    T? Get(object id, string includeProperties = "", bool tracked = false);

    void Add(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entity);
}