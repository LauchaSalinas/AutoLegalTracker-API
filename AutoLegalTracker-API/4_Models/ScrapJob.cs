using AutoLegalTracker_API._2_Business;
using AutoLegalTracker_API._5_WebServices;
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Drawing.Text;

public class ScrapJob : IJob
{
    private readonly CasoBLL caso;
    private readonly IConfiguration _configuration;
    public ScrapJob(CasoBLL casoBLL, IConfiguration configuration)
    {
        caso = casoBLL;
        _configuration = configuration;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        // Your task implementation goes here
        try
        {
            await caso.CheckNewCases();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
        Console.WriteLine("MyJob executed at: " + DateTime.Now);
        await Task.CompletedTask;
    }
}
