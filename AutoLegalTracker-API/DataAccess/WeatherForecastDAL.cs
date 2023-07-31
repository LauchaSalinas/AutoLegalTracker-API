using System;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.DataAccess

{
	public class WeatherForecastDAL : IDataAccesss<WeatherForecast>
	{
		private readonly ALTContext _context;

		public WeatherForecastDAL(ALTContext context)
        {
            _context = context;
        }

        public WeatherForecast Create(WeatherForecast entity)
        {
            _context.WeatherForecasts.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public WeatherForecast Delete(int id)
        {
            var weatherForecast = _context.WeatherForecasts.FirstOrDefault(wf => wf.Id == id);
            if (weatherForecast != null)
            {
                try
                {
                    _context.WeatherForecasts.Remove(weatherForecast);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Handle the exception and rethrow a custom application exception.
                    throw new ApplicationException("An error occurred while deleting the weather forecast.", ex);
                }
            }
            return weatherForecast;
        }

        public IEnumerable<WeatherForecast> GetAll()
        {
            return _context.WeatherForecasts.ToList();
        }

        public WeatherForecast GetById(int id)
        {
            return _context.WeatherForecasts.FirstOrDefault(wf => wf.Id == id);
        }

        public WeatherForecast Update(WeatherForecast entity)
        {
            _context.WeatherForecasts.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}

