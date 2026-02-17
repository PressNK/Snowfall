using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public class VilleService : IVilleService
{
    private readonly IVilleRepository _villeRepository;

    public VilleService(IVilleRepository villeRepository)
    {
        _villeRepository = villeRepository;
    }
    
    public async Task<List<Ville>> GetAll()
    {
        return await _villeRepository.GetAll();
    }

    public async Task<Ville?> FindById(int id)
    {
        return await _villeRepository.FindById(id);
    }
}