using System.Data;
using Dapper;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class InformationClientRepository : IInformationClientRepository
{
    private DapperContext _dbContext;

    public InformationClientRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InformationClient?> FindByUserId(string id)
    {
        string sql = @"
            SELECT * 
            FROM informations_client
            WHERE utilisateur_id = @Id;
        ";
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            return await connection.QuerySingleOrDefaultAsync<InformationClient>(
                sql, new { Id = id });
        }
    }
}