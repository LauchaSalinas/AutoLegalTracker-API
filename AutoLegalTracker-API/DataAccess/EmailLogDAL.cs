using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    //TODO i think this class doesn't need a delete o edit method 
    public class EmailLogDAL
    {
        private readonly ALTContext _context;

        public EmailLogDAL(ALTContext context)
        {
            _context = context;
        }

        #region CRUD methods

        public EmailLog Create(EmailLog entity)
        {
            if (entity != null)
            {
                try
                {
                    _context.EmailLogs.Add(entity);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred while creating the Email Log.", ex);
                }
            }

            return entity;
        }

        public IEnumerable<EmailLog> GetAll()
        {
            return _context.EmailLogs.ToList();
        }

        public EmailLog GetById(int id)
        {
            return _context.EmailLogs.FirstOrDefault(em => em.Id == id);
        }

        //TODO define if the class access to the EmailCode by a FK or string property in the Email Log class

        //Method that receives a Email Code and search in the BD
        //public EmailLog GetByEmailLogCode(string emailCode)
        //{
        //    return _context.EmailLogs.FirstOrDefault(em => em. == emailCode);
        //}

        #endregion CRUD methods
    }
}
