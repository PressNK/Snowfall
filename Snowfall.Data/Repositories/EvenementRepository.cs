using System.Data;
using Dapper;
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
            SELECT * from evenements e 
            INNER JOIN villes v ON e.Ville_Id = v.Id;
        ";

        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            IEnumerable<Evenement> evenements = await connection.QueryAsync<Evenement, Ville, Evenement>(sql,
                (evenement, ville) =>
                {
                    evenement.Ville = ville; // On lie manuellement la ville à l'événement
                    return evenement;
                });
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
                }, new { Id = id });

            // Comme Query retourne une liste, on récupère le premier élément
            var resultat = evenements.FirstOrDefault();

            return resultat;
        }
    }

    public async Task<List<Evenement>> FindByVilleId(int villeId)
    {
        string sql = @"
            SELECT * 
            FROM evenements e
            INNER JOIN villes v ON e.Ville_Id = v.Id
            WHERE v.id = @Id;
        ";
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var evenements = await connection.QueryAsync<Evenement, Ville, Evenement>(
                sql,
                (evenement, ville) =>
                {
                    evenement.Ville = ville; // On lie manuellement la ville à l'événement
                    return evenement;
                }, new { Id = villeId });

            return evenements.ToList();
        }
    }
    
    public async Task<Evenement> Create(Evenement evenement)
    {
        string sql = @"
            INSERT INTO evenements (nom, description, image_path, date, prix, capacite, ville_id)
            VALUES (@Nom, @Description, @ImagePath, @Date, @Prix, @Capacite, @VilleId)
            RETURNING id;
        ";
        
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(sql, evenement);
            evenement.Id = id;
            return evenement;
        }
    }

    public async Task<bool> Update(Evenement evenement)
    {
        string sql = @"
            UPDATE evenements SET
                nom = @Nom, 
                description = @Description, 
                image_path = @ImagePath, 
                date = @Date, 
                prix = @Prix, 
                capacite = @Capacite, 
                ville_id = @VilleId
            WHERE id = @Id
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var affectedRows = await connection.ExecuteAsync(sql, evenement);
            return affectedRows == 1;
        }
    }

    public async Task<bool> Delete(int id)
    {
        string sql = @"
            DELETE FROM evenements
            WHERE id = @Id
        ";

        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var param = new
            {
                Id = id
            };
            var affectedRows = await connection.ExecuteAsync(sql, param);
            return affectedRows == 1;
        }
    }
}