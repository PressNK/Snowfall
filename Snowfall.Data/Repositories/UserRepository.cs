using Dapper;
using Microsoft.AspNetCore.Identity;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class UserRepository : IUserRoleStore<ApplicationUser>, IUserEmailStore<ApplicationUser>,
    IUserPasswordStore<ApplicationUser>
{
    private readonly DapperContext _dbContext;

    public UserRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                INSERT INTO application_users (username, normalized_username, email,
                normalized_email, email_confirmed, password_hash, prenom, nom)
                VALUES (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @Prenom, @Nom)
                RETURNING id";
            
            user.Id = await connection.QuerySingleAsync<string>(sql, user);
        }

        return IdentityResult.Success;
    }
    
    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                UPDATE application_users SET
                    username = @UserName,
                    normalized_username = @NormalizedUserName,
                    email = @Email,
                    normalized_email = @NormalizedEmail,
                    email_confirmed = @EmailConfirmed,
                    password_hash = @PasswordHash,
                    prenom = @Prenom,
                    nom = @Nom
                WHERE id = @Id";
            
            await connection.ExecuteAsync(sql, user);
        }

        return IdentityResult.Success;
    }
    
    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"DELETE FROM application_users WHERE id = @Id";
            await connection.ExecuteAsync(sql, user);
        }

        return IdentityResult.Success;
    }
    
    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT * FROM application_users
                WHERE id = @UserId";
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { UserId = userId });
        }
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT * FROM application_users
                WHERE normalized_username = @NormalizedUsername";
            
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { NormalizedUsername = normalizedUserName });
        }
    }

    public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.Id!);
    }

    public async Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.UserName);
    }

    public async Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        if (userName != null) user.UserName = userName;
        await Task.CompletedTask;
    }

    public async Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.NormalizedUserName);
    }

    public async Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        await Task.CompletedTask;
    }

    public async Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        if (email != null) user.Email = email;
        await Task.CompletedTask;
    }

    public async Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.Email);
    }

    public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.EmailConfirmed);
    }

    public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        await Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT * FROM application_users
                WHERE normalized_email = @NormalizedEmail";
            
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { NormalizedEmail = normalizedEmail });
        }
    }

    public async Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.NormalizedEmail!);
    }

    public async Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        await Task.CompletedTask;
    }

    public async Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        await Task.CompletedTask;
    }

    public async Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.PasswordHash);
    }

    public async Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return await Task.FromResult(user.PasswordHash != null);
    }

    public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT id FROM application_roles WHERE normalized_name = @RoleName
            ";
            int? roleId = await connection.ExecuteScalarAsync<int?>(sql, new { RoleName = roleName });

            if (!roleId.HasValue)
            {
                sql = @"
                    INSERT INTO application_roles (name, normalized_name) VALUES(@RoleName, @NormalizedName)
                ";
                roleId = await connection.ExecuteAsync(sql,
                    new { RoleName = roleName, NormalizedName = roleName });
            }

            sql = @"
                INSERT INTO application_roles_users(user_id, role_id) VALUES(@UserId, @RoleId)
                ON CONFLICT DO NOTHING
            ";
            await connection.ExecuteAsync(sql, new { UserId = user.Id, RoleId = roleId });
        }
    }

    public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT id FROM application_roles WHERE normalized_name = @NormalizedName
            ";
            int? roleId = await connection.ExecuteScalarAsync<int?>(sql, new { NormalizedName = roleName.ToUpper() });
            if (roleId.HasValue)
                await connection.ExecuteAsync(@"
                    DELETE FROM application_roles_users 
                    WHERE user_id = @UserId AND role_id = @RoleId", new { UserId = user.Id, RoleId = roleId });
        }
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            var sql = @"
                SELECT r.name FROM application_roles r INNER JOIN application_roles_users ur ON ur.role_id = r.id 
                WHERE ur.user_id = @UserId";
            
            var queryResults = await connection.QueryAsync<string>(sql, new { UserId = user.Id });

            return queryResults.ToList();
        }
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            int? roleId = await connection.ExecuteScalarAsync<int?>(@"
                SELECT id FROM application_roles
                WHERE normalized_name = @NormalizedName",
                new { NormalizedName = roleName.ToUpper() });
            
            if (roleId == default(int)) return false;
            
            var matchingRoles = await connection.ExecuteScalarAsync<int>(@"
                SELECT COUNT(*) FROM application_roles_users
                WHERE user_id = @UserId AND role_id = @RoleId",
                new { UserId = user.Id, RoleId = roleId });
                
            return matchingRoles > 0;
        }
    }

    public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using (var connection = _dbContext.CreateConnection())
        {
            string sql = @"
                SELECT u.* FROM application_users u 
                INNER JOIN application_roles_users ur ON ur.user_id = u.id
                INNER JOIN application_roles r ON r.id = ur.role_id
                WHERE r.normalized_name = @NormalizedName
            ";
            
            var queryResults = await connection.QueryAsync<ApplicationUser>(sql,
                new { NormalizedName = roleName.ToUpper() });

            return queryResults.ToList();
        }
    }
    
    public void Dispose()
    {
        // Rien à disposer.
    }
}