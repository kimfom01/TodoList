using Microsoft.EntityFrameworkCore;

namespace TodoList.Data;

public class MigrationHelper
{
    public static async Task MigrateDatabaseAsync(IServiceProvider svcProvider)
    {
        //Service: An instance of db context
        var dbContextSvc = svcProvider.GetRequiredService<TodoDbContext>();

        //Migration: This is the programmatic equivalent to Update-Database
        var pendingMigrations = await dbContextSvc.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await dbContextSvc.Database.MigrateAsync();
        }
    }
}
