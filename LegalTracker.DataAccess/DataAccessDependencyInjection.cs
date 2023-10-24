using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LegalTracker.DataAccess.Persistence;
using LegalTracker.DataAccess.Repositories;
using LegalTracker.DataAccess.Repositories.Impl;

namespace LegalTracker.DataAccess;

public static class DataAccessDependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddRepositories();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<WeatherForecastRepository>();
    }
    // add type of database context to the constructor
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        //DatabaseConfiguration databaseConfig = configuration.GetSection("Database").Get<DatabaseConfiguration>();

        if (false)//(databaseConfig.UseInMemoryDatabase)
            services.AddDbContext<ScrapContext>(options =>
            {
                options.UseInMemoryDatabase("NTierDatabase");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        else
            services.AddDbContext<ScrapContext>(options =>
                options.UseSqlServer(configuration["ASPNETCORE_DBCON"],
                    opt => opt.MigrationsAssembly(typeof(ScrapContext).Assembly.FullName)));
    }

}

// TODO move outside?
public class DatabaseConfiguration
{
    public bool UseInMemoryDatabase { get; set; }

    public string ConnectionString { get; set; }
}
