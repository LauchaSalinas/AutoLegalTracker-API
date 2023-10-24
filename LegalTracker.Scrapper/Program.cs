using Microsoft.EntityFrameworkCore;
using LegalTracker.DataAccess;
using LegalTracker.DataAccess.Persistence;
using LegalTracker.Application;
using Quartz;
using LegalTracker.Scrapper.ExternalServices;
using Quartz.Impl;

namespace LegalTracker.Scrapper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddDbContext<ScrapContext>(options =>
            //{
            //    string connectionString = builder.Configuration["ASPNETCORE_DBCON"] ?? String.Empty;
            //    options.UseSqlServer(connectionString);
            //});

            builder.Services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                return schedulerFactory.GetScheduler().Result;
            });
            builder.Configuration.AddJsonFile("./ExternalServices/scrapSettings.json");

            builder.Services
                .AddDataAccess(builder.Configuration)
                .AddApplication(builder.Environment);

            builder.Services.AddControllersWithViews();

            #region Quartz

            // make that waits for jobs to complete before running the job again

            builder.Services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler-Core";
                q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";
                q.ScheduleJob<ScrapJob>(trigger => trigger
                    .WithIdentity("Combined Configuration Trigger")
                    .StartNow()
                    .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
                    .WithDescription("my awesome trigger configured for a job with single call")
                );
            });

            //Quartz.Extensions.Hosting allows you to fire background service that handles scheduler lifecycle
            builder.Services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            #endregion Quartz

            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}