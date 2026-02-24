using Dapper;
using Microsoft.AspNetCore.Identity;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class RoleRepository : IRoleStore<ApplicationRole>
{
    private DapperContext _dbContext;

    public RoleRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                INSERT INTO application_roles (name, normalized_name)
                VALUES(@Name, @NormalizedName)
                RETURNING id;
            ";

            role.Id = await connection.QuerySingleAsync<int>(sql, role);
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                UPDATE application_roles SET
                    name = @Name,
                    normalized_name = @NormalizedName
                WHERE id = @Id
            ";

            await connection.ExecuteAsync(sql, role);
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                DELETE FROM application_roles WHERE id = @Id
            ";

            await connection.ExecuteAsync(sql, role);
        }

        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name)!;
    }

    public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
    {
        if (roleName != null) role.Name = roleName;
        return Task.FromResult(0);
    }

    public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName,
        CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;
        return Task.FromResult(0);
    }

    public async Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT * FROM application_roles
                WHERE id = @RoleId
            ";

            return await connection.QuerySingleOrDefaultAsync<ApplicationRole>(sql,
                new { RoleId = Int64.Parse(roleId) });
        }
    }

    public async Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT * FROM application_roles
                WHERE normalized_name = @NormalizedRoleName
            ";

            return await connection.QuerySingleOrDefaultAsync<ApplicationRole>(sql,
                new { NormalizedRoleName = normalizedRoleName });
        }
    }

    public void Dispose()
    {
        // Rien à disposer.
    }
}