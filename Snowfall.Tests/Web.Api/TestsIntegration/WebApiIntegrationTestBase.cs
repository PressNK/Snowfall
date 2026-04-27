using Microsoft.Extensions.DependencyInjection;
using Snowfall.Data.Repositories;

namespace Snowfall.Tests.Web.Api.TestsIntegration;

/// <summary>
/// Classe de base pour les tests d'intégration d'API.
/// Gère l'injection de dépendance, la réinitialisation de la BD et le client HTTP.
/// </summary>
[Collection("Test Database Collection")]
public abstract class WebApiIntegrationTestBase :
    IClassFixture<SnowfallApiApplicationFactory>,
    IAsyncLifetime,
    IDisposable
{
    protected readonly SnowfallApiApplicationFactory Application;
    protected readonly TestDatabaseFixture Database;
    protected HttpClient Client { get; private set; } = null!;

    protected readonly IVilleRepository VilleRepository;
    protected readonly IEvenementRepository EvenementRepository;

    private readonly IServiceScope _scope;

    protected WebApiIntegrationTestBase(
        SnowfallApiApplicationFactory application,
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
}