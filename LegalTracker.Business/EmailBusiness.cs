using LegalTracker.DataAccess.Repositories;
using LegalTracker.Domain.Entities;
using LegalTracker.DataAccess.Repositories.Impl;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EmailSender;

namespace LegalTracker.Business
{
    public class EmailBusiness
    {
        private readonly IDataAccesssAsync<EmailTemplate> _dataAccess;
        private readonly IDataAccesssAsync<EmailLog> _dataAccessLog;
        private readonly EmailService _emailService;

        public EmailBusiness(IDataAccesssAsync<EmailTemplate> dataAccess, IDataAccesssAsync<EmailLog> dataAccessLog, EmailService emailService)
        {
            _emailService = emailService;   
            _dataAccessLog = dataAccessLog;
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<EmailTemplate>> GetAllEmails()
        {
            try
            {
                return await _dataAccess.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<EmailTemplate> GetEmailById(int id)
        {
            try
            {
                return await _dataAccess.GetById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<EmailTemplate> CreateEmail(EmailTemplate forecast)
        {
            try
            {
                return await _dataAccess.Insert(forecast);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task UpdateEmail(EmailTemplate forecast)
        {
            try
            {
                await _dataAccess.Update(forecast);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<EmailTemplate> DeleteEmail(int id)
        {
            try
            {
                return await _dataAccess.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        internal async Task <IEnumerable<EmailLog>> GetAllHistoricEmails()
        {
            return await _dataAccessLog.GetAll();
        }

        #region EmailLog Methods
        internal async Task <EmailLog> SendEmail(EmailTemplateEnum eventCode, string userTo)
        {
            var emailTemplate = GetAllEmails().Result.FirstOrDefault(x => x.emailCode == eventCode);

            if (emailTemplate == null){
                throw new ApplicationException("Email template not found");
            }

            // TODO fix templates  
            //_emailService.sendEmail(emailTemplate, userTo);

            var emailLog = new EmailLog {
                UserTo = userTo,
                EmailDate = DateTime.Now,
                EmailTemplateId = emailTemplate.Id
            };

            return await _dataAccessLog.Insert(emailLog);
        }

        #endregion EmailLog Methods
    }
}

