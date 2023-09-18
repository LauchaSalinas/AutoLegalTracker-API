using System;
using Quartz;

namespace AutoLegalTracker_API.Services
{
    [Obsolete("This was replaced by the Quartz DI in the Service builder")]
    public class CronService
    {
        private readonly IScheduler _scheduler;

        public CronService(IServiceProvider serviceProvider)
        {
            _scheduler = serviceProvider.GetService<IScheduler>();
        }

        public async Task StartAsync()
        {
            var job = JobBuilder.Create<ScrapJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(10)
                    .RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(job, trigger);

            await _scheduler.Start();
        }

        public async Task StopAsync()
        {
            await _scheduler.Shutdown();
        }
    }
}

