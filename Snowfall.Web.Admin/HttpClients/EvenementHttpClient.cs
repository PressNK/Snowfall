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
    
    public async Task<EvenementDto?> CreerEvenement(CreerEvenementDto creerEvenementDto)
    {
        // L'événement est null par défaut, à moins que l'appel à l'API soit un succès
        EvenementDto? evenement = null;

        // Communication avec l'API via PostAsJsonAync. On envoie le DTO.
        var response = await _client.PostAsJsonAsync(BaseApiUrl, creerEvenementDto);

        // Si la réponse est un succès, on assigne le retour à l'événement
        if (response.IsSuccessStatusCode)
        {
            evenement = await response.Content.ReadFromJsonAsync<EvenementDto>();
        }

        // Retourne null ou l'événement dans le cas d'un succès
        return evenement;
    }
    
    /// <summary>
    /// Permets de modifier un événement
    /// </summary>
    /// <param name="id">L'identifiant de l'événement à modifier</param>
    /// <param name="modifierEvenementDto">L'événement à modifier</param>
    /// <returns>Bool représentant le succès ou l'échec de la requête</returns>
    public async Task<bool> ModifierEvenement(int id, ModifierEvenementDto modifierEvenementDto)
    {
        var response = await _client.PatchAsJsonAsync($"{BaseApiUrl}/{id}", modifierEvenementDto);
        return response.IsSuccessStatusCode;
    }
    
    /// <summary>
    /// Permets de supprimer un événement
    /// </summary>
    /// <param name="id">Le id de l'événement à supprimer</param>
    /// <returns></returns>
    public async Task<bool> SupprimerEvenement(int id)
    {
        string url = $"{BaseApiUrl}/{id}";
        var response = await _client.DeleteAsync(url);

        return response.IsSuccessStatusCode;
    }
}