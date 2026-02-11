using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class EvenementRepository : IEvenementRepository
{
    private IConfiguration _configuration;
    private string? _connectionString;
    
    public EvenementRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("AppDatabaseConnection");
    }
    
    public async Task<List<Evenement>> GetAll()
    {
        string sql = @"
            SELECT * from evenements;
        ";

        using (IDbConnection connection = new NpgsqlConnection(_connectionString))
        {
            IEnumerable<Evenement> evenements = await connection.QueryAsync<Evenement>(sql);
            return evenements.ToList();
        }
    }

    public async Task<Evenement?> FindById(int id)
    {
        string sql = @"
            SELECT e.*, v.* 
            FROM evenements e
            INNER JOIN villes v ON e.Ville_Id = v.Id
            WHERE e.id = @Id;
        ";

        using (IDbConnection connection = new NpgsqlConnection(_connectionString))
        {
            var evenements = await connection.QueryAsync<Evenement, Ville, Evenement>(
                sql, 
                (evenement, ville) => {
                    evenement.Ville = ville; // On lie manuellement la ville à l'événement
                    return evenement;
                },
                new { Id = id },
                splitOn: "Id" // Indique que la deuxième table (Ville) commence à la colonne "Id"
            );

            // Comme Query retourne une liste, on récupère le premier élément
            var resultat = evenements.FirstOrDefault();

            return resultat;
        }
    }
}