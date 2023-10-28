using EmailSender;
using LegalTracker.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LegalTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Evitando referencia circular
            builder.Services.AddControllers();
                //.AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


            builder.Services.AddApplication(builder.Environment);
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            //builder.Services.AddQuartz(q =>
            //{
            //    q.SchedulerId = "Scheduler-Core";
            //    q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";
            //    q.AddJob<ScrapJob>(j => j
            //        .WithIdentity("Combined Configuration Job")
            //        .StoreDurably()
            //        .WithDescription("my awesome job configured for a single call")
            //    );
            //    q.AddTrigger(t => t
            //        .WithIdentity("Combined Configuration Trigger")
            //        .ForJob("Combined Configuration Job")
            //        .StartNow()
            //        .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Minute))
            //        .WithDescription("my awesome trigger configured for a job with single call")
            //    );
            //});

            // Quartz.Extensions.Hosting allows you to fire background service that handles scheduler lifecycle
            //builder.Services.AddQuartzHostedService(options =>
            //{
            //   // when shutting down we want jobs to complete gracefully
            //   options.WaitForJobsToComplete = true;
            //});

            // configure logging to filter entity framework messages
            builder.Logging.AddFilter((provider, category, logLevel) =>
            {
                if(category.Contains("Microsoft.EntityFrameworkCore"))
                    return false;
                return true;
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



            // if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            // {
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var Context = scope.ServiceProvider.GetRequiredService<ALTContext>();
            //        var actionBusiness = scope.ServiceProvider.GetRequiredService<ActionBusiness>();
            //        new DatabaseStartup(Context, actionBusiness).InitializeWithData();
            //    }
            // }


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