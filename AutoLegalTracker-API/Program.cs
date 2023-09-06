using AutoLegalTracker_API.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quartz.Impl;

using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;
using AutoLegalTracker_API._2_Business;
using AutoLegalTracker_API._5_WebServices;
using Quartz;
using Microsoft.Extensions.Options;
using Quartz.Impl.Calendar;
using Quartz.Impl.Matchers;
using System.Configuration;
using System.Globalization;

namespace AutoLegalTracker_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add dependency injection to the Business Logic Layer
            builder.Services.AddTransient<JwtBusiness>();
            builder.Services.AddTransient<UserBusiness>();
            builder.Services.AddTransient<WeatherForecastBLL>();
            builder.Services.AddTransient<EmailBLL>();
            builder.Services.AddTransient<CasoBLL>();
            builder.Services.AddTransient<ScrapJob>();
            // Add dependency injection to the Services Layer
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddTransient<PuppeteerService>();
            // TODO Add dependency injection to the Data Access Layer
            builder.Services.AddScoped<IDataAccesssAsync<WeatherForecast>, DataAccessAsync<WeatherForecast>>();
            builder.Services.AddScoped<IDataAccesssAsync<Email>, DataAccessAsync<Email>>();
            builder.Services.AddScoped<IDataAccesssAsync<EmailLog>, DataAccessAsync<EmailLog>>();
            
      
            // Add Quartz scheduler service
            builder.Configuration.AddJsonFile("scrapSettings.json");
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ALTContext>(options =>
            {
                string connectionString = builder.Configuration["ASPNETCORE_DBCON"] ?? String.Empty;
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

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

            // Quartz.Extensions.Hosting allows you to fire background service that handles scheduler lifecycle
            builder.Services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });


            // Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT_AUDIENCE"] ?? String.Empty,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT_ISSUER"] ?? String.Empty,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT_KEY"] ?? String.Empty))
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Create scope for EntityFramework Database migration
            using (var scope = app.Services.CreateScope())
            {
                var Context = scope.ServiceProvider.GetRequiredService<ALTContext>();

                // Context.Database.Migrate();

            }
            

            app.UseHttpsRedirection();
            app.UseCors(app => app.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}