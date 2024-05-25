#define ALWAYS_DB_RESET
using System.Text.Json;
using AspTemplate.Core.Interfaces.Repositories;
using AspTemplate.Core.Interfaces.Services;
using AspTemplate.Persistence.Mappings;
using AspTemplate.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspTemplate.Persistence.Extensions;
public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(AppDbContext)));
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddMappings();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();

        return services;
    }

    public static async Task MigrateDatabase(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("MigrateDatabase");
        try
        {
            await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

#if ALWAYS_DB_RESET
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
#else
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            logger?.LogInformation("Migrations applied successfully");
#endif


            var usersService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var createUsersResult = await usersService.CreateDefaultUsers();
            logger?.LogInformation("Default users created: {result}", JsonSerializer.Serialize(createUsersResult));

        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occured while applying migrations");
            throw;
        }
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddScoped<UserMapper>();
        services.AddScoped<MediaMapper>();

        return services;
    }
}