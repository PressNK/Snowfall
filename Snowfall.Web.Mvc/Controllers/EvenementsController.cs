using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Evenements;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class EvenementsController : Controller
{
    private IEvenementService _evenementService;
    private IVilleService _villeService;

    public EvenementsController(IEvenementService evenementService, IVilleService villeService)
    {
        _evenementService = evenementService;
        _villeService = villeService;
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
            FiltresEvenements = new FiltresEvenementsViewModel()
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

        return View(evenement);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        return Content("Vue Edit de EvenementsController, id: " + id);
    }
}