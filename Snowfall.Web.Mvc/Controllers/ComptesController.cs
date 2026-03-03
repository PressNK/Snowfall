using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Comptes;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class ComptesController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ComptesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET /comptes/inscription
    [HttpGet("[action]")]
    public IActionResult Inscription()
    {
        return View();
    }

    // POST /comptes
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InscriptionViewModel inscriptionViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                Email = inscriptionViewModel.Email,
                Nom = inscriptionViewModel.Nom,
                Prenom = inscriptionViewModel.Prenom,
                UserName = inscriptionViewModel.Email,
                InformationClient = new InformationClient
                {
                    Adresse = inscriptionViewModel.Adresse,
                    Pays =  inscriptionViewModel.Pays,
                    CodePostal = inscriptionViewModel.CodePostal,
                    Ville = inscriptionViewModel.Ville,
                    Province = inscriptionViewModel.Province,
                }
            };
            var inscriptionResult = await _userManager.CreateAsync(user, inscriptionViewModel.Password!);

            if (inscriptionResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "UTILISATEUR");
                var connexionResult = await _signInManager.PasswordSignInAsync(
                    inscriptionViewModel.Email!,
                    inscriptionViewModel.Password!,
                    true,
                    lockoutOnFailure: false
                );

                if (connexionResult.Succeeded)
                    return RedirectToAction("Index", "Evenements");
            }
            else
            {
                var hasNonAlphanumericError = inscriptionResult.Errors.Any(e => e.Code.Equals("PasswordRequiresNonAlphanumeric"));
                var key = hasNonAlphanumericError ? "Inscription.RequiresNonAlphanumeric" : "Inscription.Echec";
                var message = hasNonAlphanumericError ? Resources.Controllers.ComptesController.Inscription_RequiresNonAlphanumeric : Resources.Controllers.ComptesController.Inscription_Echec;
                ModelState.AddModelError(key, message);
            }
        }

        return View("Inscription", inscriptionViewModel);
    }
}