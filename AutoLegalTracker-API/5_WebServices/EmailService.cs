using AutoLegalTracker_API.Models;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace AutoLegalTracker_API.Services
{
    public class EmailService
    {
        #region Constructor

        private readonly string _userFrom;
        private readonly string _userName = "Juan Gonzalez";
        private readonly string _host = "smtp.gmail.com";
        private readonly bool _enableSsl = true;
        private readonly int _port = 587;
        private readonly SmtpDeliveryMethod _deliveryMethod = SmtpDeliveryMethod.Network;
        private SmtpClient _smtpClient;
        private readonly string _passwordApp;
        private readonly IConfiguration _configuration;
        

        public EmailService(IConfiguration configuration)
        {
            // Recibiendo la inyeccion de dependencia
            _configuration = configuration;
            // Indicando la dependencia a asignar
            _passwordApp = _configuration["EMAIL_SERVICE_PASSWORD"];
            _userFrom = _configuration["EMAIL_SERVICE_USERFROM"];
            createSmptClient();
        }

        #endregion Constructor

        #region Public Methods
        public bool sendEmail(Email emailTemplate, string userTo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_userFrom, _userName);
                mail.To.Add(userTo);
                mail.Subject = emailTemplate.Subject;
                mail.IsBodyHtml = false;
                mail.Body = emailTemplate.Body;

                _smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void createSmptClient()
        {
            _smtpClient = new SmtpClient(_host, _port);
            _smtpClient.EnableSsl = _enableSsl;
            _smtpClient.Port = _port;
            _smtpClient.DeliveryMethod = _deliveryMethod;
            _smtpClient.Credentials = new NetworkCredential(_userFrom, _passwordApp);
        }
    }

    #endregion Public Methods
}
