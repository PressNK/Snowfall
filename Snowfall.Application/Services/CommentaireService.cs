using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public class CommentaireService : ICommentaireService
{
    private readonly ICommentaireRepository _commentaireRepository;

    public CommentaireService(ICommentaireRepository commentaireRepository)
    {
        _commentaireRepository = commentaireRepository;
    }
    
    public async Task<Commentaire> Create(Commentaire commentaire)
    {
        return await _commentaireRepository.Create(commentaire);
    }
    
    public async Task<Commentaire?> FindById(int id)
    {
        return await _commentaireRepository.FindById(id);
    }
    
    public async Task<List<Commentaire>> FindByEvenementIdAndUserId(int evenementId, string utilisateurId)
    {
        return await _commentaireRepository.FindByEvenementIdAndUserId(evenementId, utilisateurId);
    }
    
    public async Task<List<Commentaire>> FindByEvenementId(int evenementId)
    {
        return await _commentaireRepository.FindByEvenementId(evenementId);
    }
    
    public async Task<bool> Update(Commentaire commentaire)
    {
        return await _commentaireRepository.Update(commentaire);
    }
    
    public async Task<bool> Delete(int id)
    {
        return await _commentaireRepository.Delete(id);
    }
}