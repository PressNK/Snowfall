using System.Security.Claims;
using System.Security.Principal;

namespace Snowfall.Web.Mvc.Extensions;

public static class IdentityExtensions
{
    public static string Prenom(this IIdentity identity)
    {
        return identity.FindFirstValue(ClaimTypes.GivenName);
    }
    
    public static string Nom(this IIdentity identity)
    {
        return identity.FindFirstValue(ClaimTypes.Surname);
    }

    public static string Id(this IIdentity identity)
    {
        return identity.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string NomComplet(this IIdentity identity)
    {
        return identity.FindFirstValue("NomComplet");
    }

    public static string FindFirstValue(this IIdentity identity, string claimType)
    {
        var claim = ((ClaimsIdentity)identity).FindFirst(claimType)?.Value;
        return claim ?? string.Empty;
    }
}