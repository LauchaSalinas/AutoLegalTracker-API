using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.DataAccess
{
    //Clase que hereda la interfaz IDataAcces y debe implementar obligatoriamente sus metodos
    public class EmailDAL : IDataAccesss<Email>
    {
        private readonly ALTContext _context;

        public EmailDAL(ALTContext context)
        {
            _context = context; 
        }

        #region CRUD methods

        public Email Create(Email entity)
        {
            if(entity != null)
            {
                try
                {
                    _context.Emails.Add(entity);
                    _context.SaveChanges();
                }
                catch (Exception ex) 
                {
                    throw new ApplicationException("An error occurred while creating the email template.", ex);
                }
            }

            return entity;
        }

        // This method search in the BD the object and delete them by the ID
        public Email Delete(int id)
        {
            // This method receives the first element that meets the requeriments
            var firstEmail = _context.Emails.FirstOrDefault(x => x.Id == id);
            if (firstEmail != null) 
            { 
                try
                {
                    _context.Remove(firstEmail);
                    _context.SaveChanges(); 
                }
                catch(Exception ex) 
                {
                    throw new ApplicationException("An error occurred while deleting the email template.", ex);
                }
            }
            return firstEmail; 
        }

        // This method search in the BD the object and update them by the ID
        public Email Update(Email entity)
        {
            var firstEmail = _context.Emails.FirstOrDefault(x => x.Id == entity.Id);

            if (firstEmail != null) 
            {
                try
                {
                    _context.Update(firstEmail);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred while updating the email template.", ex);
                }
            }

            return firstEmail; 
        }

        public IEnumerable<Email> GetAll()
        {
            return _context.Emails.ToList();
        }

        public Email GetById(int id)
        {
            return _context.Emails.FirstOrDefault(em => em.Id == id);
        }

        //Method that receives a Email Code and search in the BD
        public Email GetByEmailCode(string emailCode)
        {
            return _context.Emails.FirstOrDefault(em => em.EmailCode == emailCode);
        }

        #endregion CRUD methods
    }
}
