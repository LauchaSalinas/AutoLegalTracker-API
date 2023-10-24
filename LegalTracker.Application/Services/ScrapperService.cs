using LegalTracker.DataAccess.Repositories.Impl;
using LegalTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegalTracker.Application.Services
{
    public  class ScrapperService
    {
        private readonly WeatherForecastRepository _weatherForecastRepository;

        public ScrapperService(WeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
        {
            return await _weatherForecastRepository.GetAllAsync();
        }

        public async Task SeedWeatherForecasts()
        {
            var rng = new Random();
            var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = "Test"
            })
            .ToArray();

            foreach (var item in weatherForecasts)
            {
                await _weatherForecastRepository.AddAsync(item);
            }
        }
    }
}
