
using System;
using System.Linq.Expressions;

namespace LegalTracker.DataAccess.Repositories;
public interface IDataAccesssAsync<T> : IDisposable where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Insert(T entity);
    Task<T> Delete(int id);
    Task Update(T entity);
    Task<IEnumerable<T>> Query(Expression<Func<T, bool>> predicate);
}



