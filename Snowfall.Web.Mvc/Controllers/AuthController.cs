using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Auth;

namespace Snowfall.Web.Mvc.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    public AuthController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    // GET /auth/connexion
    [HttpGet("[action]")]
    public IActionResult Connexion()
    {
        return View();
    }
    
    // POST /auth
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Authentifier(ConnexionViewModel connexionViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                connexionViewModel.Email!,
                connexionViewModel.Password!,
                true,
                lockoutOnFailure: false
            );
            
            if (result.Succeeded)
                return RedirectToAction("Index", "Evenements");

            ModelState.AddModelError("Authentification.Echec", Resources.Controllers.AuthController.Authentification_Echec);
        }
        return View("Connexion", connexionViewModel);;
    }
    
    // POST /auth/deconnexion
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> Deconnexion()
    {
        await _signInManager.SignOutAsync();
    
        return RedirectToAction("Index", "Evenements");
    }
}