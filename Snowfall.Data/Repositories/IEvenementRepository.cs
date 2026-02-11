using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IEvenementRepository
{
    Task<List<Evenement>> GetAll();

    Task<Evenement?> FindById(int id);
}