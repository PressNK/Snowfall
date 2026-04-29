using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Snowfall.Tests.Web.Mvc;

public class SnowfallMvcKestrelApplicationFactory : SnowfallMvcApplicationFactory
{
    private IHost? _host;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .UseUrls("http://127.0.0.1:0");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Construit le serveur de test en mémoire (requis)
        var testHost = builder.Build();

        // Configure et construit le serveur Kestrel
        builder.ConfigureWebHost(web => web.UseKestrel());
        _host = builder.Build();
        _host.Start();

        // Récupère l'adresse du serveur Kestrel
        var server = _host.Services.GetRequiredService<IServer>();
        var address = server.Features.Get<IServerAddressesFeature>()
            .Addresses.Last();

        // Assigne comme adresse de base celle du serveur Kestrel
        ClientOptions.BaseAddress = new Uri(address);

        // Démarre le serveur de test
        testHost.Start();
        return testHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _host?.StopAsync().GetAwaiter().GetResult();
            _host?.Dispose();
        }
    }
}