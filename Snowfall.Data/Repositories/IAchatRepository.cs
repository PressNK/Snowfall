using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IAchatRepository
{
    Task<Achat> Create(Achat achat);
    Task<Achat?> FindById(int id);
    Task<Achat?> FindStripeSessionId(string id);
    Task<bool> MarquerCommePayer(int id, string paymentIntentId);
    Task<bool> MarquerCommeAnnuler(int id);
}

