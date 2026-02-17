using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public interface IEvenementService
{
    Task<List<Evenement>> GetAll();
    Task<Evenement?> FindById(int id);
    Task<List<Evenement>> FindByVilleId(int villeId);
}