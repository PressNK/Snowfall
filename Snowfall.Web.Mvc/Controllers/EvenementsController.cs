using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Evenements;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class EvenementsController : Controller
{
    private IEvenementService _evenementService;
    private IVilleService _villeService;
    private ICommentaireService _commentaireService;
    private readonly UserManager<ApplicationUser> _userManager;

    public EvenementsController(IEvenementService evenementService, IVilleService villeService, 
        ICommentaireService commentaireService, UserManager<ApplicationUser> userManager)
    {
        _evenementService = evenementService;
        _villeService = villeService;
        _commentaireService = commentaireService;
        _userManager = userManager;
    }

    [Route("/")]
    public async Task<IActionResult> Index(int? ville)
    {
        List<Evenement> evenements = ville.HasValue
            ? await _evenementService.FindByVilleId(ville.Value)
            : await _evenementService.GetAll();
        List<Ville> villes = await _villeService.GetAll();
        var viewModel = new EvenementsIndexViewModel
        {
            Evenements = evenements,
            FiltresEvenements = new FiltresEvenementsViewModel
            {
                Villes = villes,
                SelectedVilleId = ville
            }
        };
        return View(viewModel);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Show(int id, string? option)
    {
        var evenement = await _evenementService.FindById(id);

        if (evenement == null) return NotFound();
        
        var commentaires = await _commentaireService.FindByEvenementId(id);
        foreach (var commentaire in commentaires)
        {
            commentaire.Utilisateur = await _userManager.FindByIdAsync(commentaire.UtilisateurId);
        }
        
        evenement.Commentaires = commentaires;

        return View(evenement);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        return Content("Vue Edit de EvenementsController, id: " + id);
    }
}