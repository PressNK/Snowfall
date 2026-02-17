using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IVilleRepository
{
    Task<List<Ville>> GetAll();

    Task<Ville?> FindById(int id);
}