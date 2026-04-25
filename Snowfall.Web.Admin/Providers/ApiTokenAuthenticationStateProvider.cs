using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Snowfall.Web.Admin.Providers;

public class ApiTokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ApiTokenAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jetonLocalStorage = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(jetonLocalStorage))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        
        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jetonLocalStorage);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jetonLocalStorage);
    
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "jwt")));
    }
    
    public void RafraichirAuthenticationState()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    public void DeconnecterUtilisateur()
    {
        var utilisateurAnonyme = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(utilisateurAnonyme));
        NotifyAuthenticationStateChanged(authState);
    }
}