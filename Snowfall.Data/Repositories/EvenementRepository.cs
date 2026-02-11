using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class EvenementRepository : IEvenementRepository
{
    private DapperContext _dbContext;
    
    public EvenementRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Evenement>> GetAll()
    {
        string sql = @"
            SELECT * from evenements;
        ";

        using (IDbConnection connection = _dbContext.CreateConnection())
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
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var evenements = await connection.QueryAsync<Evenement, Ville, Evenement>(
                sql,
                (evenement, ville) =>
                {
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