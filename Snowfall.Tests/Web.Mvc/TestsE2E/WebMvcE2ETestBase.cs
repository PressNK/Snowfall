using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Snowfall.Data.Repositories;

namespace Snowfall.Tests.Web.Mvc.TestsE2E;

public abstract class WebMvcE2ETestBase : IClassFixture<SnowfallMvcKestrelApplicationFactory>,
    IClassFixture<TestDatabaseFixture>, IAsyncLifetime, IDisposable
{
    protected readonly SnowfallMvcKestrelApplicationFactory Application;
    protected readonly TestDatabaseFixture Database;
    protected IPage Page { get; private set; } = null!;
    protected string BaseUrl => Application.ClientOptions.BaseAddress.ToString().Trim('/');
    protected readonly IVilleRepository VilleRepository;
    protected readonly IEvenementRepository EvenementRepository;

    private IBrowser? _browser;
    private readonly IServiceScope _scope;

    protected WebMvcE2ETestBase(SnowfallMvcKestrelApplicationFactory application, TestDatabaseFixture database)
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
}