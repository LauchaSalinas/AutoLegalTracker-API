using LegalTracker.Application.Services;
using LegalTracker.Domain.Entities;
using Quartz;

namespace LegalTracker.Scrapper.ExternalServices
{
    [DisallowConcurrentExecution]
    public class ScrapJob : IJob
    {
        private readonly ScrapBusiness _scrapBusiness;

        public ScrapJob(ScrapBusiness scrapBusiness)
        {
            _scrapBusiness = scrapBusiness;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("CheckNewCasesJob executed at: " + DateTime.Now);
            
            try
            {
                await _scrapBusiness.ScrapUsersGetAllCasesAndNotifications();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("CheckNewCasesJob finished at: " + DateTime.Now);
            
            //// CheckForActionsForNewNotificationsJob
            //try
            //{
            //    await _actionBusiness.RunActionsToNewNotifications();
            //    Console.WriteLine("CheckForActionsForNewNotifications finished at: " + DateTime.Now);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}

        }
    }
}

