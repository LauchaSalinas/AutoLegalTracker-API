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
        private readonly WeatherForecastBLL _weatherForecastBLL;
        private readonly EmailService _email;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, ALTContext context, WeatherForecastBLL weatherForecastBLL, EmailService mail)
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
        public IEnumerable<EmailLog> Get()
        {
            Email email = new Email();
            email.EmailCode = "EMAIL-CODE-PROTOTIPO-3";
            email.Subject = "SUBJECT-PROTOTIPO-3";
            email.Body = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500";

            _context.Emails.Add(email);
            _context.SaveChanges();

            //TRYING SEND EMAIL ON GET
            //var dataEmail = _email.sendEmail("This its a test", "gonzalez01juanm@gmail.com", "Lorem Ipsum");

            EmailLog emailLog = new EmailLog();
            emailLog.EmailDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            emailLog.UserTo = "gonzalez01juanm@gmail.com";
            emailLog.EmailId = 3;

            // Adding in the DB the emailLog created
            _context.EmailLogs.Add(emailLog);
            _context.SaveChanges();


            //return emailLog; 
            // return the log emails
            return _context.EmailLogs;
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
            _context.WeatherForecasts.Add(weatherForecast);
            // save the changes 
            _context.SaveChanges();

        }

            //[Authorize]
            //[HttpDelete("{id}", Name = "DeleteWeatherForecast")]
            //public IActionResult Delete(int id)
            //{
            //    try
            //    {
            //        // TODO add Delete Method
            //        var weatherForecast = _weatherForecastBLL.DeleteWeatherForecast(id);
            //        if (weatherForecast == null)
            //            return NotFound();

            //        return Ok(weatherForecast);
            //    }
            //    catch (ApplicationException ex)
            //    {
            //        // Log the exception
            //        _logger.LogError(ex, "An error occurred while deleting the weather forecast.");
            //        // Return a custom application error
            //        return BadRequest("An error occurred while deleting the weather forecast.");
            //    }
            //}
    }
}