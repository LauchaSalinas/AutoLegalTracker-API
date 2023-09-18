using AutoLegalTracker_API.Business;
using Quartz;
using System.Runtime.CompilerServices;

public class ScrapJob : IJob
{
    private readonly ScrapBusiness _scrapBusiness;
    private readonly ActionBusiness _actionBusiness;
    public ScrapJob(ScrapBusiness scrapBusiness, ActionBusiness actionBusiness)
    {
        _scrapBusiness = scrapBusiness;
        _actionBusiness = actionBusiness;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        // CheckNewCasesJob
        Console.WriteLine("CheckNewCasesJob executed at: " + DateTime.Now);
        try
        {
            await _scrapBusiness.CheckNewCases();
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
    static async Task EmulateAsyncMethod(int secondsDelay)
    {
        await Task.Delay(secondsDelay*1000);
    }
}
