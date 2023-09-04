using Quartz;
using System;
using System.Threading.Tasks;

public class ScrapJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // Your task implementation goes here

        Console.WriteLine("ScrapJob started at: " + DateTime.Now);
        await EmulateAsyncMethod(10); // Emulate 10 seconds delay
        Console.WriteLine("ScrapJob finished at: " + DateTime.Now);

        Console.WriteLine("MedicalAppointmentAssignationJob started at: " + DateTime.Now);
        await EmulateAsyncMethod(10); // Emulate 10 seconds delay
        Console.WriteLine("MedicalAppointmentAssignationJob finished at: " + DateTime.Now);
        await Task.CompletedTask;
    }
    static async Task EmulateAsyncMethod(int secondsDelay)
    {
        await Task.Delay(secondsDelay*1000);
    }
}
