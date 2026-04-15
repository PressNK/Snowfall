using Snowfall.Web.Admin.HttpClients;

namespace Snowfall.Web.Admin.Configurations;

public static class InjectionDependancesConfig
{
    public static IServiceCollection EnregistrerServices(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<EvenementHttpClient>();
        services.AddScoped<VilleHttpClient>();

        return services;
    }
}