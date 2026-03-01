using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IQuestionRepository
{
    Task<Question> Create(Question question);
    Task<Question?> FindById(int id);
    Task<List<Question>> FindByEvenementIdAndUserId(int evenementId, string utilisateurId);
    Task<bool> Update(Question question);
    Task<bool> Delete(int id);
}