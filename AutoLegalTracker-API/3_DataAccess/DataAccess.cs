using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    public class DataAccessAsync<T> : IDataAccesssAsync<T> where T : class
    {
        private readonly ALTContext _context;

        public DataAccessAsync(ALTContext context)
        {
            _context = context;
        }

        protected async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        protected DbSet<T> EntitySet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public async Task<T> Delete(int id)
        {
            try
            {
                var entity = await EntitySet.FindAsync(id);
                if (entity == null)
                {
                    throw new ApplicationException($"Entity with ID {id} not found.");
                }

                EntitySet.Remove(entity);
                await Save();

                return entity;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while deleting entity.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await EntitySet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while fetching entities.", ex);
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                var entity = await EntitySet.FindAsync(id);
                if (entity == null)
                {
                    throw new ApplicationException($"Entity with ID {id} not found.");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while fetching entity by ID.", ex);
            }
        }

        public async Task<T> Insert(T entity)
        {
            try
            {
                EntitySet.Add(entity);
                await Save();
                return entity;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while inserting entity.", ex);
            }
        }

        public async Task Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await Save();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<T>> Query(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await EntitySet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while querying entities.", ex);
            }
        }
    }
}
