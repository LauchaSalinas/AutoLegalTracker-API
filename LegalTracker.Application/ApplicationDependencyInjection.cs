using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LegalTracker.Application.Services;
using EmailSender;
//using LegalTracker.Application.Services.DevImpl;
//using LegalTracker.Application.Services.Impl;
//using LegalTracker.Shared.Services;
//using LegalTracker.Shared.Services.Impl;

namespace LegalTracker.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddServices(env);

        services.RegisterAutoMapper();

        return services;
    }

    private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<ScrapperService>();

        //if (env.IsDevelopment())
        //    services.AddScoped<IEmailService, DevEmailService>();
        //else
        //    services.AddScoped<IEmailService, EmailService>();
    }

    private static void RegisterAutoMapper(this IServiceCollection services)
    {
        //services.AddAutoMapper(typeof(IMappingProfilesMarker));
    }

    
}
