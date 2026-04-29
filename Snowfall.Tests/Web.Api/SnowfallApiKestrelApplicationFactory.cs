using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Snowfall.Tests.Web.Api;

public class SnowfallApiKestrelApplicationFactory : WebApplicationFactory<Snowfall.Web.Api.Program>
{
    private IHost? _host;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Force l'utilisation de la configuration de test
        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile(
                "appsettings.Test.json",
                optional: true,
                reloadOnChange: false);
        });
        
        var testHost = builder.Build();

        // Configure le vrai serveur Kestrel
        builder.ConfigureWebHost(web => web.UseKestrel());
        _host = builder.Build();
        _host.Start();

        // Alloue un port aléatoire à Kestrel
        var server   = _host.Services.GetRequiredService<IServer>();
        var address  = server.Features.Get<IServerAddressesFeature>()!
            .Addresses.Last();

        // Faire pointer l'adresse de base vers celle de Kestrel
        ClientOptions.BaseAddress = new Uri(address);

        // Démarrer le serveur de test
        testHost.Start();

        return testHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            if (_host is not null)
            {
                _host.StopAsync().GetAwaiter().GetResult();
                _host.Dispose();
            }
        }
    }
}