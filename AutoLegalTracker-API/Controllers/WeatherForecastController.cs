using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Business;

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

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastBLL weatherForecastBLL)
        {
            _logger = logger;
            _weatherForecastBLL = weatherForecastBLL;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            // return all the weather forecasts
            return await _weatherForecastBLL.GetAllWeatherForecasts();
        }

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