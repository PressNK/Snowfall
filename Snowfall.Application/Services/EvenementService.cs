using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public class EvenementService : IEvenementService
{
    private readonly IEvenementRepository _evenementRepository;

    public EvenementService(IEvenementRepository evenementRepository)
    {
        _evenementRepository = evenementRepository;
    }
    
    public async Task<List<Evenement>> GetAll()
    {
        return await _evenementRepository.GetAll();
    }

    public async Task<Evenement?> FindById(int id)
    {
        return await _evenementRepository.FindById(id);
    }
    
    public async Task<List<Evenement>> FindByVilleId(int villeId)
    {
        return await _evenementRepository.FindByVilleId(villeId);
    }
    
    public async Task<Evenement> Create(Evenement evenement)
    {
        return await _evenementRepository.Create(evenement);
    }
    
    public async Task<bool> Update(Evenement evenement)
    {
        return await _evenementRepository.Update(evenement);
    }
}