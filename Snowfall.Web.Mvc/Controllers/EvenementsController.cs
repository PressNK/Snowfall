using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class EvenementsController : Controller
{
    private IEvenementService _evenementService;

    public EvenementsController(IEvenementService evenementService)
    {
        _evenementService = evenementService;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        List<Evenement> evenements = await _evenementService.GetAll();
        return View(evenements);
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