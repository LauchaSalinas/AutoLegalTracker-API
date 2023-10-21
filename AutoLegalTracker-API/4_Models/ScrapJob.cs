using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.DataAccess;
using Quartz;
using System.Linq;

namespace AutoLegalTracker_API.Models
{
    [DisallowConcurrentExecution]
    public class ScrapJob : IJob
    {
        private readonly ScrapBusiness _scrapBusiness;
        private readonly ActionBusiness _actionBusiness;
        private readonly LegalCaseDataAccessAsync _legalCaseDataAccess;
        private readonly LegalNotificationDataAccess _legalNotificationDataAccess;
        private readonly UserDataAccess _userDataAccess;
        public ScrapJob(ScrapBusiness scrapBusiness, ActionBusiness actionBusiness, LegalCaseDataAccessAsync legalCaseDataAccess, LegalNotificationDataAccess legalNotificationDataAccess, UserDataAccess userDataAccess)
        {
            _scrapBusiness = scrapBusiness;
            _actionBusiness = actionBusiness;
            _legalCaseDataAccess = legalCaseDataAccess;
            _legalNotificationDataAccess = legalNotificationDataAccess;
            _userDataAccess = userDataAccess;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // CheckNewCasesJob
            Console.WriteLine("CheckNewCasesJob executed at: " + DateTime.Now);
            try
            {
                //await _scrapBusiness.CheckNewCases();
                
                IEnumerable<User> usersToScrap = await _userDataAccess.GetUsersToScrapAsync();
                foreach (var user in usersToScrap)
                {
                    try
                    {
                        await _scrapBusiness.LogIn(user);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        throw;
                    }

                    //try
                    //{
                    //    var lastScrappedLegalCase = await _legalCaseDataAccess.GetLastScrappedLegalCase(user.Id);
                    //    var lastScrappedPage = user.LastScrappedPage;
                    //    lastScrappedLegalCase = lastScrappedLegalCase ?? new LegalCase { CaseNumber = "0" };
                    //    (IEnumerable<LegalCase> legalCasesToAdd, var newLastScrappedPage) = await _scrapBusiness.ScrapUserGetNewCases(user, lastScrappedLegalCase.CaseNumber, lastScrappedPage);
                    //    await _legalCaseDataAccess.AddRangeAsync(legalCasesToAdd);
                    //    await _userDataAccess.UpdateLastScrappedPage(user, newLastScrappedPage);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.ToString());
                    //    throw;
                    //    // TODO handle TargetClosedException and other exceptions
                    //}

                    //try
                    //{
                    //    IEnumerable<LegalCase> legalCasesToUpdate = await _legalCaseDataAccess.GetCasesToScrap(user.Id);
                    //    foreach (var legalCase in legalCasesToUpdate)
                    //    {
                    //        var lastNotification = await _legalNotificationDataAccess.GetLastNotification(legalCase.Id);
                    //        try{
                    //            var legalNotificationsToAdd = await _scrapBusiness.ScrapCaseGetNewNotifications(legalCase, lastNotification);
                    //            await _legalNotificationDataAccess.AddRangeAsync(legalNotificationsToAdd);
                    //            // update last scrapped at
                    //            legalCase.LastScrappedAt = DateTime.Now;
                    //            await _legalCaseDataAccess.UpdateLastScrappedAt(legalCase);
                    //            Console.WriteLine($"Caso cargado {legalCase.Id} ");

                    //        }
                    //        catch(Exception ex)
                    //        {
                    //            Console.WriteLine($"Error de carga caso {legalCase.Id} " + ex.ToString());
                    //        }
                            
                    //    }
                    //}
                    //catch(Exception ex)
                    //{
                    //    Console.WriteLine(ex.ToString());
                    //    throw;
                    //}

                    //try
                    //{
                    //    IEnumerable<LegalCase> legalCasesWithEmptyNotifications = await _legalCaseDataAccess.GetCasesWithEmptyNotifications(user.Id);
                    //    foreach (var legalCase in legalCasesWithEmptyNotifications)
                    //    {
                    //        var legalNotificationsToFill = await _legalNotificationDataAccess.GetEmptyNotifications(legalCase.Id);
                    //        var legalNotificationsToUpdate = await _scrapBusiness.ScrapNotificationsUpdateContent(legalNotificationsToFill);
                    //        await _legalNotificationDataAccess.UpdateRangeAsync(legalNotificationsToUpdate);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.ToString());
                    //    throw;
                    //}



                    // TODO add user to the function
                    // TODO pass date to the function, recursively last 5 days.
                    for (int pastDaysToScrap = 1 ; pastDaysToScrap <= 5; pastDaysToScrap++) 
                    {
                        try
                        {
                            var legalCasesFromNotificationPage = await _scrapBusiness.SaveCasesByNotificationsPage(user.Id, pastDaysToScrap);

                            foreach (var legalCase in legalCasesFromNotificationPage)
                            {
                                var lastNotification = await _legalNotificationDataAccess.GetLastNotification(legalCase.Id);
                                try
                                {
                                    var legalNotificationsToAdd = await _scrapBusiness.ScrapCaseGetNewNotifications(legalCase, lastNotification);
                                    await _legalNotificationDataAccess.AddRangeAsync(legalNotificationsToAdd);
                                    // update last scrapped at
                                    legalCase.LastScrappedAt = DateTime.Now;
                                    await _legalCaseDataAccess.UpdateLastScrappedAt(legalCase);
                                    Console.WriteLine($"Caso cargado {legalCase.Id} ");

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error de carga caso {legalCase.Id} " + ex.ToString());
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            throw;
                        }
                    }

                    await _scrapBusiness.LogOut();
                }
                

                Console.WriteLine("CheckNewCasesJob finished at: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // CheckForActionsForNewNotificationsJob
            try
            {
                await _actionBusiness.RunActionsToNewNotifications();
                Console.WriteLine("CheckForActionsForNewNotifications finished at: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}

