using Snowfall.Domain.Models;

namespace Snowfall.Data.Repositories;

public interface IInformationClientRepository
{
    Task<InformationClient?> FindById(string id);
}