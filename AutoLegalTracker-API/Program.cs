using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quartz.Impl;

using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;
using AutoLegalTracker_API.WebServices;
using AutoLegalTracker_API.DataAccess;
using Quartz;

namespace AutoLegalTracker_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            #region Dependency Injection

            // Add dependency injection to the Business Logic Layer
            builder.Services.AddTransient<JwtBusiness>();
            builder.Services.AddTransient<UserBusiness>();
            builder.Services.AddTransient<WeatherForecastBusiness>();
            builder.Services.AddTransient<EmailBusiness>();
            builder.Services.AddTransient<CalendarBusiness>();
            builder.Services.AddTransient<MedicalAppointmentBusiness>();


            builder.Services.AddTransient<CasoBLL>();
            builder.Services.AddTransient<ScrapJob>();
            // Add dependency injection to the Services Layer
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<GoogleOAuth2Service>();
            builder.Services.AddScoped<GoogleCalendarService>();
            builder.Services.AddScoped<GoogleDriveService>();
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddTransient<PuppeteerService>();
            // TODO Add dependency injection to the Data Access Layer
            builder.Services.AddScoped<IDataAccesssAsync<WeatherForecast>, DataAccessAsync<WeatherForecast>>();
            builder.Services.AddScoped<IDataAccesssAsync<EmailTemplate>, DataAccessAsync<EmailTemplate>>();
            builder.Services.AddScoped<IDataAccesssAsync<EmailLog>, DataAccessAsync<EmailLog>>();
            builder.Services.AddScoped<IDataAccesssAsync<User>, DataAccessAsync<User>>();
            builder.Services.AddScoped<IDataAccesssAsync<MedicalAppointment>, DataAccessAsync<MedicalAppointment>>();
            builder.Services.AddScoped<IDataAccesssAsync<Models.Calendar>, DataAccessAsync<Models.Calendar>>();


            builder.Services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                return schedulerFactory.GetScheduler().Result;
            });

            

            // Add Quartz scheduler service
            builder.Configuration.AddJsonFile("scrapSettings.json");
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion Dependency Injection

            builder.Services.AddDbContext<ALTContext>(options =>
            {
                string connectionString = builder.Configuration["ASPNETCORE_DBCON"] ?? String.Empty;
                options.UseSqlServer(connectionString);
            });

            #region CORS and Security

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", corsBuilder =>
                {
                    corsBuilder.WithOrigins(builder.Configuration["JWT_AUDIENCE"] ?? String.Empty)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["AuthToken"];
                        return Task.CompletedTask;
                    }
                };
            });

            #endregion CORS and Security

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
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
            app.UseCors(app => app.WithOrigins(builder.Configuration["JWT_AUDIENCE"] ?? String.Empty).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

            // TODO Lsalinas: manage DI resources
            // Use appropriate lifetime management: Depending on the nature of the dependency,
            // you can manage its lifetime appropriately. For example, you can use a Singleton
            // scope for a service that should be shared across the application, or you can use
            // a Scoped scope for a service that should have a shorter lifespan(e.g., within a
            // single HTTP request).By managing the lifetime correctly, you can avoid unnecessary
            // resource allocation.

            // Lazy initialization: If a dependency represents a resource that should be allocated
            // only when needed, you can use lazy initialization.In C#, you can use Lazy<T> to
            // achieve this. The resource will only be created when it's accessed for the first time.
        }
    }
}