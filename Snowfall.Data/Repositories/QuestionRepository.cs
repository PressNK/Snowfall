using System.Data;
using Dapper;
using Snowfall.Data.Context;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private DapperContext _dbContext;
    
    public QuestionRepository(DapperContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Question> Create(Question question)
    {
        string sql = @"
            INSERT INTO questions (utilisateur_id, evenement_id, contenu, created_at, updated_at)
            VALUES (@UtilisateurId, @EvenementId, @Contenu, @CreatedAt, @UpdatedAt)
            RETURNING id;
        ";

        question.CreatedAt = DateTime.Now;
        question.UpdatedAt = question.CreatedAt;
        
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(sql, question);
            question.Id = id;
            return question;
        }
    }
    
    public async Task<List<Question>> FindByEvenementIdAndUserId(int evenementId, string userId)
    {
        string sql = @"
            SELECT * FROM questions q
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
            IEnumerable<Question> questions = await connection.QueryAsync<Question>(sql, param);
            return questions.ToList();
        }
    }
    
    public async Task<Question?> FindById(int id)
    {
        string sql = @"
            SELECT * FROM questions
            WHERE id = @Id
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            var param = new
            {
                Id = id
            };
            Question? question = await connection.QuerySingleOrDefaultAsync<Question>(sql, param);
            return question;
        }
    }
    
    public async Task<bool> Update(Question question)
    {
        string sql = @"
            UPDATE questions SET
                utilisateur_id = @UtilisateurId,
                evenement_id = @EvenementId,
                contenu = @Contenu,
                updated_at = @UpdatedAt
            WHERE id = @Id
        ";
    
        using (IDbConnection connection = _dbContext.CreateConnection())
        {
            question.UpdatedAt = DateTime.Now;

            var affectedRows = await connection.ExecuteAsync(sql, question);
            return affectedRows == 1;
        }
    }

    public async Task<bool> Delete(int id)
    {
        string sql = @"
            DELETE FROM questions
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