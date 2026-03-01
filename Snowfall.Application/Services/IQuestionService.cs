using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public interface IQuestionService
{
    Task<Question> Create(Question question);
    Task<Question?> FindById(int id);
    Task<List<Question>> FindByEvenementIdAndUserId(int evenementId, string utilisateurId);
    Task<bool> Update(Question question);
    Task<bool> Delete(int id);
}