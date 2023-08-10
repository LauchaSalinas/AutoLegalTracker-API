using Quartz;
using System;
using System.Threading.Tasks;

public class ScrapJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // Your task implementation goes here
        Console.WriteLine("MyJob executed at: " + DateTime.Now);
        await Task.CompletedTask;
    }
}
