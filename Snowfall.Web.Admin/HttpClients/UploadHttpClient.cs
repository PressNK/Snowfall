using System.Net.Http.Json;
using Snowfall.Application.Dtos.Uploads;

namespace Snowfall.Web.Admin.HttpClients;

public class UploadHttpClient
{
    private const string BaseApiUrl = "api/uploads";
    private readonly HttpClient _client;
        
    public UploadHttpClient(HttpClient client)
    {
        _client = client;
    }
        
    public async Task<ResultatUpload?> Upload(MultipartFormDataContent content)
    {
        ResultatUpload? resultatUpload = null;
        
        var reponse = await _client.PostAsync(BaseApiUrl, content);

        // Lire la réponse si un succès et retourner le contenu de la réponse
        if (reponse.IsSuccessStatusCode)
        {
            resultatUpload = reponse.Content.ReadFromJsonAsync<ResultatUpload>().Result;
        }
        
        return resultatUpload;
    }
}