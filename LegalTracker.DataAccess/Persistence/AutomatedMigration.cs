using LegalTracker.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LegalTracker.DataAccess.Persistence;

public static class AutomatedMigration
{
    public static async Task MigrateAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ScrapContext>();

        if (context.Database.IsSqlServer()) 
            await context.Database.MigrateAsync();
    }
}
