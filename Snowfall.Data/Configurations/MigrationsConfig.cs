using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Snowfall.Data.Configurations;

public static class MigrationsConfig
{
    /// <summary>
    /// Enregistre FluentMigrator à la collection de services
    /// </summary>
    public static IServiceCollection AddMigrations(this IServiceCollection services, string connectionString)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Configure le service FluentMigrator
        services.AddFluentMigratorCore()
            .ConfigureRunner(c => c
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.All()
            );

        // Ajoute le logging en console
        services.AddLogging(c => c.AddFluentMigratorConsole());

        return services;
    }

    /// <summary>
    /// Exécute les migrations "Up"
    /// </summary>
    public static void MigrateUp(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateUp();
    }
    
    /// <summary>
    /// Exécute les migrations "Down"
    /// </summary>
    public static void MigrateDown(this IServiceProvider serviceProvider, long version = 0)
    {
        using var scope = serviceProvider.CreateScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateDown(version);
    }
}