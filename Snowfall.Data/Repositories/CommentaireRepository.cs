using System.Data;
using Dapper;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class CommentaireRepository : ICommentaireRepository
{
    private DapperContext _dbContext;
    
    public CommentaireRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Commentaire> Create(Commentaire commentaire)
    {
        string sql = @"
            INSERT INTO commentaires (utilisateur_id, evenement_id, contenu, created_at, updated_at)
            VALUES (@UtilisateurId, @EvenementId, @Contenu, @CreatedAt, @UpdatedAt)
            RETURNING id;
        ";

        commentaire.CreatedAt = DateTime.Now;
        commentaire.UpdatedAt = commentaire.CreatedAt;
        
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(sql, commentaire);
            commentaire.Id = id;
            return commentaire;
        }
    }
    
    public async Task<List<Commentaire>> FindByEvenementIdAndUserId(int evenementId, string userId)
    {
        string sql = @"
            SELECT * FROM commentaires q
            WHERE q.evenement_id = @EvenementId
            AND q.utilisateur_id = @UserId
            ORDER BY created_at DESC
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var param = new
            {
                EvenementId = evenementId,
                UserId = userId
            };
            IEnumerable<Commentaire> commentaires = await connection.QueryAsync<Commentaire>(sql, param);
            return commentaires.ToList();
        }
    }
    
    public async Task<List<Commentaire>> FindByEvenementId(int evenementId)
    {
        string sql = @"
            SELECT * FROM commentaires q
            WHERE q.evenement_id = @EvenementId
            ORDER BY created_at DESC
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var param = new
            {
                EvenementId = evenementId
            };
            IEnumerable<Commentaire> commentaires = await connection.QueryAsync<Commentaire>(sql, param);
            return commentaires.ToList();
        }
    }
    
    public async Task<Commentaire?> FindById(int id)
    {
        string sql = @"
            SELECT * FROM commentaires
            WHERE id = @Id
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var param = new
            {
                Id = id
            };
            Commentaire? commentaire = await connection.QuerySingleOrDefaultAsync<Commentaire>(sql, param);
            return commentaire;
        }
    }
    
    public async Task<bool> Update(Commentaire commentaire)
    {
        string sql = @"
            UPDATE commentaires SET
                utilisateur_id = @UtilisateurId,
                evenement_id = @EvenementId,
                contenu = @Contenu,
                updated_at = @UpdatedAt
            WHERE id = @Id
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            commentaire.UpdatedAt = DateTime.Now;

            var affectedRows = await connection.ExecuteAsync(sql, commentaire);
            return affectedRows == 1;
        }
    }

    public async Task<bool> Delete(int id)
    {
        string sql = @"
            DELETE FROM commentaires
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