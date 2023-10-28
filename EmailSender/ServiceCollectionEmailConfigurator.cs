using Microsoft.Extensions.DependencyInjection;

namespace EmailSender;

public static class ServiceCollectionEmailConfigurator
{
    public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, Action<SmtpSettings> options)
    {
        var emailConfiguration = new SmtpSettings();
        options(emailConfiguration);
        services.AddSingleton(emailConfiguration);

        return services;
    }
}
