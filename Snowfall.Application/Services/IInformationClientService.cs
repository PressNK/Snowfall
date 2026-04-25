using Snowfall.Domain.Models;

namespace Snowfall.Application.Services;

public interface IInformationClientService
{
    Task<InformationClient?> FindByUserId(string id);
}