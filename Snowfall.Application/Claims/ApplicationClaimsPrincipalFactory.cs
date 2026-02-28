using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Claims;

/// <summary>
/// Permets de générer les attributs de l'utilisateur (Claims) étant accessibles sans faire de requête
/// à la BD. Ces claims sont donc stockés dans un cookie, par exemple (en mode rendu serveur), et récupérés
/// par le serveur à chaque requête.
/// </summary>
public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    public ApplicationClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> options)
        : base(userManager, roleManager, options)
    {
    }

    /// <summary>
    /// Génère les claims propres à l'application. Cette méthode 'override' celle de
    /// UserClaimsPrincipalFactory. C'est pourquoi on appel base.GenerateClaimsAsync()
    /// dans un premier temps. 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        ClaimsIdentity claims = await base.GenerateClaimsAsync(user);
        
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id!));
        claims.AddClaim(new Claim(ClaimTypes.GivenName, user.Prenom));
        claims.AddClaim(new Claim(ClaimTypes.Surname, user.Nom));
        claims.AddClaim(new Claim("NomComplet", $"{user.Prenom} {user.Nom}"));

        return claims;
    }
}