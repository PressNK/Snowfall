using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public interface IVilleService
{
    Task<List<Ville>> GetAll();
    Task<Ville?> FindById(int id);
}