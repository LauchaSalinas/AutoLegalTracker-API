using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoLegalTracker_API.DataAccess;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Business
{
    public class WeatherForecastBusiness
    {
        private readonly IDataAccesssAsync<WeatherForecast> _dataAccess;

        public WeatherForecastBusiness(IDataAccesssAsync<WeatherForecast> dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<WeatherForecast>> GetAllWeatherForecasts()
        {
            try
            {
                return await _dataAccess.GetAll();
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw;
            }
        }

        public async Task<WeatherForecast> GetWeatherForecastById(int id)
        {
            try
            {
                return await _dataAccess.GetById(id);
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw;
            }
        }

        public async Task<WeatherForecast> CreateWeatherForecast(WeatherForecast forecast)
        {
            try
            {
                return await _dataAccess.Insert(forecast);
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw;
            }
        }

        public async Task UpdateWeatherForecast(WeatherForecast forecast)
        {
            try
            {
                await _dataAccess.Update(forecast);
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw;
            }
        }

        public async Task<WeatherForecast> DeleteWeatherForecast(int id)
        {
            try
            {
                return await _dataAccess.Delete(id);
            }
            catch (Exception ex)
            {
                // Handle the exception
                throw;
            }
        }
    }
}
