using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public interface ICommentaireService
{
    Task<Commentaire> Create(Commentaire commentaire);
    Task<Commentaire?> FindById(int id);
    Task<List<Commentaire>> FindByEvenementIdAndUserId(int evenementId, string utilisateurId);
    Task<List<Commentaire>> FindByEvenementId(int evenementId);
    Task<bool> Update(Commentaire commentaire);
    Task<bool> Delete(int id);
}