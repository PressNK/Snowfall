using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public class AchatService : IAchatService
{
    private readonly IAchatRepository _achatRepository;

    public AchatService(IAchatRepository achatRepository)
    {
        _achatRepository = achatRepository;
    }

    public async Task<Achat> Create(Achat achat)
    {
        return await _achatRepository.Create(achat);
    }

    public async Task<Achat?> FindById(int id)
    {
        return await _achatRepository.FindById(id);
    }
    
    public async Task<Achat?> FindStripeSessionId(string id)
    {
        return await _achatRepository.FindStripeSessionId(id);
    }
    
    public async Task<bool> MarquerCommePayer(int id, string paymentIntentId)
    {
        return await _achatRepository.MarquerCommePayer(id, paymentIntentId);
    }
    
    public async Task<bool> MarquerCommeAnnuler(int id)
    {
        return await _achatRepository.MarquerCommeAnnuler(id);
    }
}

