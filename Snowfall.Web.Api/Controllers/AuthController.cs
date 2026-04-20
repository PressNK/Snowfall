using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Snowfall.Application.Dtos.Auth;
using Snowfall.Domain.Models;

namespace Snowfall.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        
        
        /// <summary>
        /// POST /api/auth
        /// Permets d'authentifier un utilisateur et de retourner un jeton d'authentification (JWT)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Connexion(ConnexionDto connexionDto)
        {
            var utilisateur = await _userManager.FindByNameAsync(connexionDto.Email!);

            if (utilisateur == null)
                return Unauthorized();
            
            var resultat = await _signInManager.CheckPasswordSignInAsync(
                utilisateur,
                connexionDto.Password!,
                false
            );

            if (!resultat.Succeeded)
                return Unauthorized();
            
            string token = await CreerToken(utilisateur);

            return Ok(new ResultatConnexionDto() { Token = token });
        }
        
        /// <summary>
        /// Permets de créer un jeton JWT à partir d'un utilisateur 
        /// </summary>
        /// <param name="utilisateur">L'utilisateur pour qui créer le jeton</param>
        /// <returns>Jeton JWT au format string</returns>
        private async Task<string> CreerToken(ApplicationUser utilisateur)
        {
            // La clé secrète est récupérée de la configuration (appsettings) 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]!));
    
            // On crée une clé de signature à partir de la clé secrète
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            // La date d'expiration du jeton est configurée en fonction de la durée en jours du jeton
            DateTime expirationDateTime = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpirationJours"]!));
            
            // Les attributs de l'utilisateur qu'on veut rendre disponible côté client via le jeton
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, utilisateur.UserName),
                new Claim(ClaimTypes.GivenName, utilisateur.Prenom),
                new Claim(ClaimTypes.Surname, utilisateur.Nom),
                new Claim(JwtRegisteredClaimNames.Email, utilisateur.Email),
                new Claim(JwtRegisteredClaimNames.Sub, utilisateur.Id!),
            };
            
            var roles = await _userManager.GetRolesAsync(utilisateur);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expirationDateTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
