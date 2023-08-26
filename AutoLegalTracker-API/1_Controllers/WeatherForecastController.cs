using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoLegalTracker_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastBusiness _weatherForecastBLL;
        private readonly EmailBusiness _email;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, ALTContext context, WeatherForecastBusiness weatherForecastBLL, EmailBusiness mail)
        {
            _email = mail;
            _logger = logger;
            _weatherForecastBLL = weatherForecastBLL;
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
    }
}
