using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public class InformationClientService : IInformationClientService
{
    private readonly IInformationClientRepository _informationClientRepository;

    public InformationClientService(IInformationClientRepository informationClientRepository)
    {
        _informationClientRepository = informationClientRepository;
    }

    public async Task<InformationClient?> FindById(string id)
    {
        return await _informationClientRepository.FindById(id);
    }
}