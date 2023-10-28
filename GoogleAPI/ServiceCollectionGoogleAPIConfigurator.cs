using Microsoft.Extensions.DependencyInjection;

namespace GoogleAPI;

public static class ServiceCollectionGoogleAPIConfigurator
{
    public static IServiceCollection AddGoogleAPIConfiguration(this IServiceCollection services, Action<GoogleApiSettings> options)
    {
        var googleApiSettings = new GoogleApiSettings();
        options(googleApiSettings);
        services.AddSingleton(googleApiSettings);

        return services;
    }
}
