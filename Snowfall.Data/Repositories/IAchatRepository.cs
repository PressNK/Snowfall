using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IAchatRepository
{
    Task<Achat> Create(Achat achat);
    Task<Achat?> FindById(int id);
}

