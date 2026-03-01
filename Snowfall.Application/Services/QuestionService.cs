using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }
    
    public async Task<Question> Create(Question question)
    {
        return await _questionRepository.Create(question);
    }
    
    public async Task<Question?> FindById (int id)
    {
        return await _questionRepository.FindById(id);
    }
    
    public async Task<List<Question>> FindByEvenementIdAndUserId (int evenementId, string utilisateurId)
    {
        return await _questionRepository.FindByEvenementIdAndUserId(evenementId, utilisateurId);
    }
    
    public async Task<bool> Update (Question question)
    {
        return await _questionRepository.Update(question);
    }
    
    public async Task<bool> Delete (int id)
    {
        return await _questionRepository.Delete(id);
    }
}