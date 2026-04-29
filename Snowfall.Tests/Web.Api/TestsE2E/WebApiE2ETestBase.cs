using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Snowfall.Application.Dtos.Auth;
using Snowfall.Data.Repositories;

namespace Snowfall.Tests.Web.Api.TestsE2E;

[Collection("Test Database Collection")]
public abstract class WebApiE2ETestBase : IClassFixture<SnowfallApiKestrelApplicationFactory>, IClassFixture<TestDatabaseFixture>, IAsyncLifetime, IDisposable
{
    protected readonly SnowfallApiKestrelApplicationFactory Application;
    protected readonly TestDatabaseFixture Database;
    protected IPage Page { get; private set; } = null!;
    protected string BaseUrl => Application.ClientOptions.BaseAddress.ToString().Trim('/');
    protected readonly IVilleRepository VilleRepository;
    protected readonly IEvenementRepository EvenementRepository;
    
    private IBrowser? _browser;
    private readonly IServiceScope _scope;

    protected WebApiE2ETestBase(SnowfallApiKestrelApplicationFactory application, TestDatabaseFixture database)
    {
        Application = application;
        Database = database;
        
        _scope = Application.Services.CreateScope();
        VilleRepository = _scope.ServiceProvider.GetRequiredService<IVilleRepository>();
        EvenementRepository = _scope.ServiceProvider.GetRequiredService<IEvenementRepository>();
    }

    public async Task InitializeAsync()
    {
        // Réinitialise la base de données
        await Database.ResetDatabaseAsync();

        // Démarre le serveur
        var client = Application.CreateClient();

        // Lance Playwright et crée une nouvelle page
        var playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync(new 
            BrowserTypeLaunchOptions
            {
                Headless = false 
                
            });
        var context = await _browser.NewContextAsync();
        Page = await context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_browser is not null)
            await _browser.CloseAsync();
    }

    public void Dispose()
    {
    }
    
    protected async Task AuthentifierUtilisateur(string email, string password)
    {
        // Effectuer un appel de connexion à l'API pour obtenir un jeton
        var connextionDto = new ConnexionDto {
            Email = email,
            Password = password
        };
        var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        var response   = await httpClient.PostAsJsonAsync("/api/auth", connextionDto);
        response.EnsureSuccessStatusCode();

        // Extraire le jeton de la réponse
        var resultat = await response.Content.ReadFromJsonAsync<ResultatConnexionDto>(); 
        string token = resultat!.Token;

        // Ajouter le jeton au localStorage de la page
        await Page.Context.AddInitScriptAsync($@"
        window.localStorage.setItem('authToken', '{token}')
    ");
    }

}