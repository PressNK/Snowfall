using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public interface IAchatService
{
    Task<Achat> Create(Achat achat);
    Task<Achat?> FindById(int id);
}

