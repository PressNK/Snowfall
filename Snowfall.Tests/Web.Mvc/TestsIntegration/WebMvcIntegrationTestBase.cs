using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Snowfall.Data.Repositories;
using Snowfall.Tests.Helpers;

namespace Snowfall.Tests.Web.Mvc.TestsIntegration;

[Collection("Test Database Collection")]
public abstract class WebMvcIntegrationTestBase :
    IClassFixture<SnowfallMvcApplicationFactory>,
    IAsyncLifetime,
    IDisposable
{
    protected readonly SnowfallMvcApplicationFactory Application;
    protected readonly TestDatabaseFixture Database;
    protected HttpClient Client { get; private set; } = null!;

    protected readonly IVilleRepository VilleRepository;
    protected readonly IEvenementRepository EvenementRepository;

    private readonly IServiceScope _scope;

    protected WebMvcIntegrationTestBase(
        SnowfallMvcApplicationFactory application,
        TestDatabaseFixture database)
    {
        Application = application;
        Database = database;
        
        _scope = Application.Services.CreateScope();
        VilleRepository = _scope.ServiceProvider.GetRequiredService<IVilleRepository>();
        EvenementRepository = _scope.ServiceProvider.GetRequiredService<IEvenementRepository>();
    }

    /// <summary>
    /// Avant chaque test, crée un nouveau client et réinitialise la BD.
    /// </summary>
    public async Task InitializeAsync()
    {
        await Database.ResetDatabaseAsync();
        Client = Application.CreateClient();
    }
    
    public Task DisposeAsync() => Task.CompletedTask;
    
    public void Dispose() => _scope.Dispose();
    
    public async Task<HttpClient> CreerClientUtilisateur(string email, string password)
    {
        var client = this.Application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        HttpResponseMessage reponse = await client.GetAsync("Auth/Connexion");
        IHtmlDocument pageConnexion = await HtmlHelpers.GetDocumentAsync(reponse);
    
        reponse = await client.SendAsync(
            (pageConnexion.QuerySelector("main form") as IHtmlFormElement)!,
            (pageConnexion.QuerySelector("main form button.btn-primary") as IHtmlButtonElement)!,
            new Dictionary<string, string>
            {
                { "Email", email },
                { "Password", password },
            });

        return client;
    }
}