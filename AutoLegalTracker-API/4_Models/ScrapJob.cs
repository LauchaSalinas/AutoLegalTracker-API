using AutoLegalTracker_API._2_Business;
using Quartz;
using System;
using System.Threading.Tasks;

public class ScrapJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // Your task implementation goes here
        CasoBLL caso = new();
        await caso.checkNewCases();
        Console.WriteLine("MyJob executed at: " + DateTime.Now);
        await Task.CompletedTask;
    }
}
