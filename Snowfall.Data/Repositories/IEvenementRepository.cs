using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IEvenementRepository
{
    Task<List<Evenement>> GetAll();
    Task<Evenement?> FindById(int id);
    Task<List<Evenement>> FindByVilleId(int villeId);
    Task<Evenement> Create(Evenement evenement);
    Task<bool> Update(Evenement evenement);
    Task<bool> Delete(int id);
}