using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Achats;
using Snowfall.Web.Mvc.Models.Panier;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class AchatsController : Controller
{
    private readonly IEvenementService _evenementService;
    private readonly IInformationClientService _informationClientService;
    private readonly IAchatService _achatService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AchatsController(IEvenementService evenementService, IInformationClientService informationClientService, IAchatService achatService,
        UserManager<ApplicationUser> userManager)
    {
        _evenementService = evenementService;
        _informationClientService = informationClientService;
        _achatService = achatService;
        _userManager = userManager;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Checkout()
    {
        // 1. Obtenir le contenu du panier de la session
        var panierJson = HttpContext.Session.GetString("panier");
        if (string.IsNullOrEmpty(panierJson))
            return RedirectToAction("Index", "Panier");

        var panierItems = JsonSerializer.Deserialize<List<PanierItemViewModel>>(panierJson) ?? [];

        // 2. Récupérer les evenements associés au panier à partir de la BD
        foreach (var item in panierItems)
            item.Evenement = await _evenementService.FindById(item.ItemId);

        // 3. Récupérer l'utilisateur connecté et ses informations
        var user = await _userManager.GetUserAsync(User);
        
        if (user == null)
            return RedirectToAction("Connexion", "Auth");

        var informationsClient = await _informationClientService.FindById(user.Id!);
        var informationClientModel = new InformationClientModel()
        {
            Adresse = informationsClient!.Adresse,
            Ville = informationsClient!.Ville,
            CodePostal = informationsClient!.CodePostal,
            Province = informationsClient!.Province,
            Pays = informationsClient!.Pays
        };

        // 4. Effectuer les calculs nécessaires
        var sousTotal = panierItems.Sum(i => (i.Evenement?.Prix ?? 0) * i.Quantite);
        var livraison = sousTotal < 100m ? 9.99m : 0m;
        var total = sousTotal + livraison;

        // 5. Construire le ViewModel
        var viewModel = new CheckoutViewModel
        {
            Items = panierItems,
            Utilisateur = user,
            SousTotal = sousTotal,
            Livraison = livraison,
            Total = total,
            InformationClient = informationClientModel
        };

        return View(viewModel);
    }

    [HttpPost("[action]")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        // 1. Obtenir le contenu du panier de la session
        var panierJson = HttpContext.Session.GetString("panier");
        if (string.IsNullOrEmpty(panierJson))
            return RedirectToAction("Index", "Panier");

        var panierItems = JsonSerializer.Deserialize<List<PanierItemViewModel>>(panierJson) ?? [];

        // 2. Récupérer les evenements associés
        foreach (var item in panierItems)
            item.Evenement = await _evenementService.FindById(item.ItemId);

        // 3. Récupérer l'utilisateur connecté
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        // 4. Calculs
        var sousTotal = panierItems.Sum(i => (i.Evenement?.Prix ?? 0) * i.Quantite);
        var livraison = sousTotal < 100m ? 9.99m : 0m;
        var total = sousTotal + livraison;

        // 5. Construire l'achat et sauvegarder dans la BD (avec transaction dans le repository)
        var achat = new Achat
        {
            UtilisateurId = user.Id!,
            SousTotal = sousTotal,
            Livraison = livraison,
            Total = total,
            LignesAchat = panierItems.Select(i => new LigneAchat
            {
                EvenementId = i.ItemId,
                Quantite = i.Quantite,
                PrixUnitaire = i.Evenement?.Prix ?? 0
            }).ToList()
        };

        var achatCree = await _achatService.Create(achat);

        // 6. Vider le panier de la session
        HttpContext.Session.Remove("panier");

        // 7. Rediriger vers la page de confirmation
        return RedirectToAction("Confirmation", new { id = achatCree.Id });
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Confirmation(int id)
    {
        // 1. Récupérer l'achat
        var achat = await _achatService.FindById(id);
        if (achat == null) return NotFound();

        // 2. Récupérer l'utilisateur
        achat.Utilisateur = await _userManager.GetUserAsync(User);

        return View(achat);
    }
}

