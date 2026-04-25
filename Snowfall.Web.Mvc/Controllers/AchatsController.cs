using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Achats;
using Snowfall.Web.Mvc.Models.Panier;
using Stripe.Checkout;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class AchatsController : Controller
{
    private readonly IEvenementService _evenementService;
    private readonly IInformationClientService _informationClientService;
    private readonly IAchatService _achatService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SessionService _stripeSessionService;

    public AchatsController(IEvenementService evenementService, IInformationClientService informationClientService, IAchatService achatService,
        UserManager<ApplicationUser> userManager,  SessionService stripeSessionService)
    {
        _evenementService = evenementService;
        _informationClientService = informationClientService;
        _achatService = achatService;
        _userManager = userManager;
        _stripeSessionService = stripeSessionService;
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

        var informationsClient = await _informationClientService.FindByUserId(user.Id!);
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
        
        var items = panierItems.Select(i => new LigneAchat
        {
            EvenementId = i.ItemId,
            Quantite = i.Quantite,
            PrixUnitaire = i.Evenement?.Prix ?? 0,
            EvenementNom = i.Evenement?.Nom
        }).ToList();
        
        var lineItems = new List<SessionLineItemOptions>();
        foreach (var item in items)
        {
            var sessionLineItemOptions = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.PrixUnitaire * 100),  // 99.99 = 9999 sans virgule,
                    Currency = "cad",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.EvenementNom,
                    },
                },
                Quantity = item.Quantite,
            };
            lineItems.Add(sessionLineItemOptions);
        }
        
        
        // ouverture de session stripe
        var options = new SessionCreateOptions
        {
            LineItems = lineItems,
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            AutomaticTax = new SessionAutomaticTaxOptions
            {
                Enabled = true
            },
            Mode = "payment",
            SuccessUrl = Url.Action("Confirmation", "Achats", null, Request.Scheme) + "?sessionId={CHECKOUT_SESSION_ID}", // URL de retour ici
            CancelUrl = Url.Action("Annuler", "Achats", null, Request.Scheme) + "?sessionId={CHECKOUT_SESSION_ID}" // URL en cas d'annulation
        };
        Session session = _stripeSessionService.Create(options);

        // 5. Construire l'achat et sauvegarder dans la BD (avec transaction dans le repository)
        var achat = new Achat
        {
            UtilisateurId = user.Id!,
            SousTotal = sousTotal,
            Livraison = livraison,
            Total = total,
            StatutPaiement = StatutPaiement.Attente,
            StripeSessionId = session.Id,
            LignesAchat = panierItems.Select(i => new LigneAchat
            {
                EvenementId = i.ItemId,
                Quantite = i.Quantite,
                PrixUnitaire = i.Evenement?.Prix ?? 0,
                EvenementNom = i.Evenement?.Nom
            }).ToList()
        };

        var achatCree = await _achatService.Create(achat);

        // 7. Rediriger vers la page stripe
        return new RedirectResult(session.Url);
    }

    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> Confirmation([FromQuery]  string sessionId)
    {
        // 1. Récupérer l'achat
        var achat = await _achatService.FindStripeSessionId(sessionId);
        if (achat == null) return NotFound();

        // 2. Récupérer l'utilisateur
        achat.Utilisateur = await _userManager.GetUserAsync(User);
        var informationsClient = await _informationClientService.FindByUserId(achat.Utilisateur!.Id!);
        var informationClientModel = new InformationClientModel()
        {
            Adresse = informationsClient!.Adresse,
            Ville = informationsClient!.Ville,
            CodePostal = informationsClient!.CodePostal,
            Province = informationsClient!.Province,
            Pays = informationsClient!.Pays
        };
        
        Session session = _stripeSessionService.Get(sessionId);
        if (session.PaymentStatus == "paid")
        {
            await _achatService.MarquerCommePayer(achat.Id, session.PaymentIntentId);
            HttpContext.Session.Remove("panier");
            ConfirmationViewModel viewModel = new ConfirmationViewModel
            {
                Achat = achat,
                InformationClient = informationClientModel
            };
            return View(viewModel);
        }
        return BadRequest();
    }
    
    [HttpGet("[Action]")]
    public async Task<IActionResult> Annuler([FromQuery]  string sessionId)
    {
        // 1. Récupérer l'achat
        var achat = await _achatService.FindStripeSessionId(sessionId);
        if (achat == null) return NotFound();
        
        Session session = _stripeSessionService.Get(sessionId);
        
        if (session.PaymentStatus != "paid")
        {
            await _achatService.MarquerCommeAnnuler(achat.Id);
        }
        
        return RedirectToAction("Index", "Panier");
    }
}

