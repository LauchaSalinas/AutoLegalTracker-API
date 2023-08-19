using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AutoLegalTracker_API.Business
{
    public class EmailBLL
    {
        private readonly IDataAccesssAsync<EmailTemplate> _dataAccess;
        private readonly IDataAccesssAsync<EmailLog> _dataAccessLog;
        private readonly EmailService _emailService;

        public EmailBLL(IDataAccesssAsync<EmailTemplate> dataAccess, IDataAccesssAsync<EmailLog> dataAccessLog, EmailService emailService)
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
                // Handle the exception
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
                // Handle the exception
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
                // Handle the exception
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
                // Handle the exception
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
                // Handle the exception
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

            _emailService.sendEmail(emailTemplate, userTo);

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

