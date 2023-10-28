using System.Net;
using System.Net.Mail;

namespace EmailSender
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
        private readonly SmtpSettings _smptSettings;


        public EmailService(SmtpSettings smptSettings)
        {
            // Recibiendo la inyeccion de dependencia
            _smptSettings = smptSettings;
            // Indicando la dependencia a asignar
            createSmptClient();
        }

        #endregion Constructor

        #region Public Methods
        public bool sendEmail(EmailTemplateDTO emailTemplate, string userTo)
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
            _smtpClient = new SmtpClient(_host, _port)
            {
                EnableSsl = _enableSsl,
                Port = _port,
                DeliveryMethod = _deliveryMethod,
                Credentials = new NetworkCredential(_userFrom, _passwordApp)
            };
        }
    }

    #endregion Public Methods
}
