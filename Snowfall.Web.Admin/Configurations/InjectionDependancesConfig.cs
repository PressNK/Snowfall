using Microsoft.AspNetCore.Components.Authorization;
using Snowfall.Web.Admin.HttpClients;
using Snowfall.Web.Admin.Providers;

namespace Snowfall.Web.Admin.Configurations;

public static class InjectionDependancesConfig
{
    public static IServiceCollection EnregistrerServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<EvenementHttpClient>();
        services.AddScoped<VilleHttpClient>();
        services.AddScoped<UploadHttpClient>();
        services.AddScoped<AuthHttpClient>();
        services.AddScoped<AuthenticationStateProvider, ApiTokenAuthenticationStateProvider>();

        return services;
    }
}