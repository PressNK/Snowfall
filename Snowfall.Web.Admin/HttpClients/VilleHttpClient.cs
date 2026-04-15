using System.Net;
using System.Net.Http.Json;
using Snowfall.Application.Dtos.Evenements;

namespace Snowfall.Web.Admin.HttpClients;

public class VilleHttpClient
{
    private const string BaseApiUrl = "api/villes";
    private readonly HttpClient _client;
    
    public VilleHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<List<VilleDto>?> ObtenirVilles()
    {
        return await _client.GetFromJsonAsync<List<VilleDto>>(BaseApiUrl);
    }
    
    public async Task<(HttpStatusCode, VilleDto?)> ObtenirVille(int id)
    {
        string url = $"{BaseApiUrl}/{id}";
        var reponse = await _client.GetAsync(url);
        var statusCode = reponse.StatusCode;
        VilleDto? ville = null;
    
        if (reponse.IsSuccessStatusCode)
        {
            ville = await reponse.Content.ReadFromJsonAsync<VilleDto>();
        }

        return (statusCode, ville);
    }
}