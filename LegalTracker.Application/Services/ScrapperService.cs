using LegalTracker.DataAccess.Repositories.Impl;
using LegalTracker.Domain.DTOs;
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
        public Task<UserToScrapNewLegalCasesDTO> GetUserToScrapAllCases()
        {
            throw new NotImplementedException();
        }

        public Task UpdateScrappedUserAddLegalCases(UserToScrapNewLegalCasesDTO scrappedUser)
        {
            throw new NotImplementedException();
        }

        public Task<UsersLegalCasesToScrapNotificationsDTO> GetUserWithEmptyCasesToScrap()
        {
            throw new NotImplementedException();
        }

        public Task UpdateLegalCasesAddNotifications(LegalCaseToScrapNotificationsDTO user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFilledLegalNotification(LegalNotificationToFillDTO filledLegalNotification)
        {
            throw new NotImplementedException();
        }

        public Task<UsersLegalNotificationsToUpdateContentDTO> GetUserToScrapNotificationContent()
        {
            throw new NotImplementedException();
        }

        public Task<UserToScrapLastNotificationsDTO> GetUserToScrapLastNotifications()
        {
            throw new NotImplementedException();
        }

        public void SaveLegalNotificationsByNotificationsPage(IEnumerable<LegalNotificationByNotificationsPageDTO> legalNotificationsFroNewNotificationPage)
        {
            // Should try to find the legal case, if not found, save it to the table orphan legal notifications
            throw new NotImplementedException();
        }

        public Task<UserToScrapOrphanNotificationsDTO> GetUserToScrapOrphanNotifications()
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateLegalCase(OrphanNotificationDTO orphanNotification)
        {
            // check if case exist, if not, add it
            // if exist update it by adding the notification
            throw new NotImplementedException();
        }
    }
}
