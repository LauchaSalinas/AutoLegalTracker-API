using Microsoft.AspNetCore.Mvc;
using LegalTracker.Domain.Entities;
using LegalTracker.Application.Services;

namespace LegalTracker.Scrapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ScrapperService _scrapperService;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ScrapperService scrapperService)
        {
            _logger = logger;
            _scrapperService = scrapperService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            await _scrapperService.SeedWeatherForecasts();
            return await _scrapperService.GetWeatherForecasts();
        }
    }
}