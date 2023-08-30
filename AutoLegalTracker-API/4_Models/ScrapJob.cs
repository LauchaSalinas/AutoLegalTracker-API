using AutoLegalTracker_API._2_Business;
using AutoLegalTracker_API._5_WebServices;
using Quartz;
using System;
using System.Threading.Tasks;

public class ScrapJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // Your task implementation goes here
        var puppeteer = new PuppeteerService();
        CasoBLL caso = new(puppeteer);
        try
        {
            await caso.checkNewCases();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
        Console.WriteLine("MyJob executed at: " + DateTime.Now);
        await Task.CompletedTask;
    }
}
