using System.Net.Http.Json;
using Snowfall.Application.Dtos.Auth;

namespace Snowfall.Web.Admin.HttpClients;

public class AuthHttpClient
{
    private const string BaseApiUrl = "api/auth";
        
    private readonly HttpClient _client;
        
    public AuthHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<ResultatConnexionDto?> Connexion(ConnexionDto connexionDto)
    {
        ResultatConnexionDto? resultatConnexionDto = null;
        
        var response = await _client.PostAsJsonAsync(BaseApiUrl, connexionDto);
        
        if (response.IsSuccessStatusCode)
        {
            resultatConnexionDto = await response.Content.ReadFromJsonAsync<ResultatConnexionDto>();
        }

        return resultatConnexionDto;
    }
    
    
}