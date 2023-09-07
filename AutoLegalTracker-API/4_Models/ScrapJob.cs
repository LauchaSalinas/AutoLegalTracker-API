using AutoLegalTracker_API.Business;
using Quartz;

public class ScrapJob : IJob
{
    private readonly ScrapBusiness caso;
    public ScrapJob(ScrapBusiness ScrapBusiness)
    {
        caso = ScrapBusiness;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("MyJob executed at: " + DateTime.Now);
        // Your task implementation goes here
        try
        {
            await caso.CheckNewCases();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        

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
