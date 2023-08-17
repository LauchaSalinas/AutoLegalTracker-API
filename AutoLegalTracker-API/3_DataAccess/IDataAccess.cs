using System;
namespace AutoLegalTracker_API.DataAccess
{
	public interface IDataAccesssAsync<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Insert(T entity);
        Task<T> Delete(int id);
        Task Update(T entity);
    }
}

