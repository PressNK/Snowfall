using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Snowfall.Data.Context;

/// <summary>
/// Permets d'ouvrir et d'obtenir des connexions à la BD permettant de faire des requêtes.
/// </summary>
public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AppDatabaseConnection")!;
    }

    /// <summary>
    /// Crée une connexion à la BD
    /// </summary>
    /// <returns>Connexion IDbConnection</returns>
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}