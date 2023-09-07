using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AutoLegalTracker_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastBusiness _weatherForecastBLL;
        private readonly EmailBusiness _email;
        private readonly ALTContext _context;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ALTContext context, WeatherForecastBusiness weatherForecastBLL, EmailBusiness mail, IConfiguration configuration)
        {
            _email = mail;
            _logger = logger;
            _weatherForecastBLL = weatherForecastBLL;
            _context = context;
            _configuration = configuration;
        }
        //[HttpGet(Name = "GetWeatherForecast")]
        //public async Task<IEnumerable<WeatherForecast>> Get()
        //{
        // return all the weather forecasts
        //    return await _weatherForecastBLL.GetAllWeatherForecasts();

        //[Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task <IEnumerable<EmailLog>> Get()
        {
            //Email email = new Email();
            //email.emailCode = EmailCode.NewCase; 
            //email.Subject = "SUBJECT-PROTOTIPO-1";
            //email.Body = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500";

            //await _email.CreateEmail(email);

            //await _email.SendEmail(EmailCode.NewCase, "gonzalez01juanm@gmail.com");

            var historicMails = await _email.GetAllHistoricEmails();

            return historicMails; 
        }
        
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    //TRYING SEND EMAIL ON GET
        //    //var dataEmail = _email.sendEmail("This its a test", "gonzalez01juanm@gmail.com", "Lorem Ipsum");

        //    //Email email = new Email();
        //    //email.EmailCode = "EMAIL-CODE-PROTOTIPO-2";
        //    //email.Subject = "SUBJECT-PROTOTIPO-2"; 
        //    //email.Body = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el tex
        //    //to de relleno estándar de las industrias desde el año 1500";

        //    //_context.Emails.Add(email);
        //    //_context.SaveChanges();

        //    // return all the weather forecasts
        //    return _context.WeatherForecasts;
        //}


        [HttpPost]
        public async Task Post(int year, int month, int day, int temperature, string summary)
        {
            // instantiate a new weatherforecast object
            WeatherForecast weatherForecast = new WeatherForecast();
            // set the properties of the object
            weatherForecast.Date = new DateTime(year, month, day);
            weatherForecast.TemperatureC = temperature;
            weatherForecast.Summary = summary;



            // add the object to the database
            await _weatherForecastBLL.CreateWeatherForecast(weatherForecast);
        }

        [Authorize]
        [HttpDelete("{id}", Name = "DeleteWeatherForecast")]
        public IActionResult Delete(int id)
        {
            try
            {
                var weatherForecast = _weatherForecastBLL.DeleteWeatherForecast(id);
                if (weatherForecast == null)
                    return NotFound();

                return Ok(weatherForecast);
            }
            catch (ApplicationException ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while deleting the weather forecast.");
                // Return a custom application error
                return BadRequest("An error occurred while deleting the weather forecast.");
            }
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {

            // create a user
            var user = new User
            {
                Sub = "user123", // Example value for Sub
                Name = "Juan",
                FamilyName = "Gonzalez",
                Email = "juan@example.com",
                GoogleProfilePicture = "https://example.com/juan_profile.jpg",
                WebCredentialUser = "web_user", // Example value for WebCredentialUser
                WebCredentialPassword = "web_password", // Example value for WebCredentialPassword
                GoogleOAuth2RefreshToken = "refresh_token", // Example value for GoogleOAuth2RefreshToken
                GoogleOAuth2AccessToken = "access_token", // Example value for GoogleOAuth2AccessToken
                GoogleOAuth2TokenExpiration = 1234567890, // Example value for GoogleOAuth2TokenExpiration
                GoogleOAuth2TokenCreatedAt = DateTime.UtcNow, // Current UTC time as an example
                GoogleOAuth2IdToken = "id_token", // Example value for GoogleOAuth2IdToken
                LegalCases = new List<LegalCase>() // You can initialize this if needed
            };

            // create a LegalCase
            var legalCase = new LegalCase
            {
                Caption = "Caso 1",
                Description = "Caso 1",
                CreatedAt = DateTime.UtcNow, // Example value for CreatedAt
                User = user, // Assign the User navigation property
                Jurisdiction = "Example Jurisdiction", // Example value for Jurisdiction
                CaseNumber = "123ABC", // Example value for CaseNumber
            };
            user.LegalCases.Add(legalCase);

            // create a LegalAutomation
            var legalAutomation = new LegalAutomation
            {
                Name = "Automatizacion 1",
                Description = "Automatizacion 1",
            };

            // create a LegalNotification
            var legalNotification = new LegalNotification
            {
                Title = "Notificacion 1",
                Description = "Notificacion 1",
                LegalCase = legalCase,
                LegalAutomation = legalAutomation
            };

            legalAutomation.LegalNotifications.Add(legalNotification);

            // create a MedicalAppointment
            var medicalAppointment = new MedicalAppointment
            {
                Name = "Cita 1",
                Description = "Cita 1",
                LegalNotification = legalNotification
            };

            // store it in the database
            _context.Users.Add(user);

            _context.SaveChanges();
            return Ok("Test");
        }

        [AllowAnonymous]
        [HttpGet("test2")]
        public IActionResult Test2()
        {
            // get every case of every user
            var users = _context.Users.Include(u => u.LegalCases).ToList();
            return Ok("Test");


            //Use Projection(Select) to Shape Data:
            //Sometimes you might not need all the properties of related entities.In such cases, you can use projection to retrieve only the required data.
            //var usersWithCaseNames = dbContext.Users
            //    .Select(u => new
            //    {
            //        u.Name,
            //        LegalCaseNames = u.LegalCases.Select(c => c.Name)
            //    })
            //    .ToList();
        }
    }
}
