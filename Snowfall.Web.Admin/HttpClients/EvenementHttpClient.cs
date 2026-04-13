using System.Net;
using System.Net.Http.Json;
using Snowfall.Application.Dtos.Evenements;

namespace Snowfall.Web.Admin.HttpClients;

public class EvenementHttpClient
{
    private const string BaseApiUrl = "api/evenements";
    private readonly HttpClient _client;
    
    public EvenementHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<List<EvenementDto>?> ObtenirEvenements()
    {
        return await _client.GetFromJsonAsync<List<EvenementDto>>(BaseApiUrl);
    }
    
    public async Task<(HttpStatusCode, EvenementDto?)> ObtenirEvenement(int id)
    {
        string url = $"{BaseApiUrl}/{id}";
        var reponse = await _client.GetAsync(url);
        var statusCode = reponse.StatusCode;
        EvenementDto? evenement = null;
    
        if (reponse.IsSuccessStatusCode)
        {
            evenement = await reponse.Content.ReadFromJsonAsync<EvenementDto>();
        }

        return (statusCode, evenement);
    }
}