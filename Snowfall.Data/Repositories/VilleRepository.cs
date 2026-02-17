using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class VilleRepository : IVilleRepository
{
    private DapperContext _dbContext;
    
    public VilleRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Ville>> GetAll()
    {
        string sql = @"
            SELECT * from villes;
        ";

        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            IEnumerable<Ville> villes = await connection.QueryAsync<Ville>(sql);
            return villes.ToList();
        }
    }

    public async Task<Ville?> FindById(int id)
    {
        string sql = @"
            SELECT *
            FROM villes
            WHERE id = @Id;
        ";
        
        
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var ville = await connection.QuerySingleOrDefaultAsync<Ville>(sql, new { Id = id });
            return ville;
        }
    }
}