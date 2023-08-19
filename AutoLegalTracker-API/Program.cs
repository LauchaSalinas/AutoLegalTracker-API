using AutoLegalTracker_API.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quartz.Impl;
using Microsoft.OpenApi.Models;

using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Models;
using AutoLegalTracker_API.Services;



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
            // Add dependency injection to the Services Layer
            builder.Services.AddTransient<EmailService>();
            // TODO Add dependency injection to the Data Access Layer
            builder.Services.AddScoped<IDataAccesssAsync<WeatherForecast>, DataAccessAsync<WeatherForecast>>();
            builder.Services.AddScoped<IDataAccesssAsync<EmailTemplate>, DataAccessAsync<EmailTemplate>>();
            builder.Services.AddScoped<IDataAccesssAsync<EmailLog>, DataAccessAsync<EmailLog>>();
            builder.Services.AddScoped<IDataAccesssAsync<User>, DataAccessAsync<User>>();


            builder.Services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                return schedulerFactory.GetScheduler().Result;
            });

            // Add Quartz scheduler service
            builder.Services.AddSingleton<CronService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Add the Bearer token authorization option
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                // Make sure the security requirement includes the "Bearer" option
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

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

                Context.Database.Migrate();

                // Start Quartz scheduler
                var quartzService = scope.ServiceProvider.GetRequiredService<CronService>();
                quartzService.StartAsync().Wait();
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