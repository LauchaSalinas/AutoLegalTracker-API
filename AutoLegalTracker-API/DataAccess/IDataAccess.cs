using System;
namespace AutoLegalTracker_API.DataAccess
{
	public interface IDataAccesss<T>
    {
        T Create(T entity);
        T Update(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        T Delete(int id);
    }
}

