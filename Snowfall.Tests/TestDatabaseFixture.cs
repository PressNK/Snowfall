using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Snowfall.Data.Configurations;

namespace Snowfall.Tests;

public class TestDatabaseFixture : IAsyncLifetime
{
    private NpgsqlConnection? _connection;
    private Respawner? _respawner;
    private string? _connectionString;

    public async Task InitializeAsync()
    {
        // Charger la configuration de l'application
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        _connectionString = config.GetConnectionString("AppDatabaseConnection")!;
        
        // Respawn doit avoir un schéma de base de données de prêt, on s'assure que la BD est migrée
        EnsureDatabaseMigrated();
        
        // Ouvrir une connection SQL pour Respawn
        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync();
        
        // Créer l'instance de Respawn
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter      = DbAdapter.Postgres,
            TablesToIgnore = new Table[]
            {
                "VersionInfo",
                "application_users",
                "application_roles",
                "application_roles_users"
            }
        });
    }
    
    /// <summary>
    /// Exécute les migrations afin d'assurer un schéma de base de données
    /// de départ complet
    /// </summary>
    private void EnsureDatabaseMigrated()
    {
        if (_connectionString is not null)
        {
            var services = new ServiceCollection()
                .AddMigrations(_connectionString);
            var sp = services.BuildServiceProvider();
            sp.MigrateUp();   
        }
    }

    /// <summary>
    /// Ferme la connexion SQL
    /// </summary>
    public async Task DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
    }
    
    /// <summary>
    /// Réinitialise la base de données (données seulement).
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        if (_connection is not null && _respawner is not null)
        {
            await _respawner.ResetAsync(_connection);   
        }
    }
    
    [CollectionDefinition("Test Database Collection")]
    public class TestDatabaseCollection : ICollectionFixture<TestDatabaseFixture>
    {
        // Classe utilisée seulement pour la collection xUnit
    }
}