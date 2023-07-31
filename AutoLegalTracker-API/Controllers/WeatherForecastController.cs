using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoLegalTracker_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ALTContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ALTContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // return all the weather forecasts
            return _context.WeatherForecasts;
        }

        [HttpPost]
        public void Post(int year, int month, int day, int temperature, string summary)
        {
            // instantiate a new weatherforecast object
            WeatherForecast weatherForecast = new WeatherForecast();
            // set the properties of the object
            weatherForecast.Date = new DateTime(year, month, day);
            weatherForecast.TemperatureC = temperature;
            weatherForecast.Summary = summary;
            // add the object to the database
            _context.WeatherForecasts.Add(weatherForecast);
            // save the changes
            _context.SaveChanges();

        }

        [Authorize]
        [HttpDelete("{id}", Name = "DeleteWeatherForecast")]
        public IActionResult Delete(int id)
        {
            try
            {
                var weatherForecast = _weatherForecastService.DeleteWeatherForecast(id);
                if (weatherForecast == null)
                    return NotFound();

                return Ok(weatherForecast);
            }
            catch (AppDataAccessException ex)
            {
                // Log the exception or handle it accordingly.
                // Return an appropriate error response to the client.
                return BadRequest("An error occurred while deleting the weather forecast.");
            }
        }
    }
}