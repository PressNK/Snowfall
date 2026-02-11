using Microsoft.AspNetCore.Mvc;
using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class EvenementsController : Controller
{
    private IEvenementRepository _evenementRepository;

    public EvenementsController(IEvenementRepository evenementRepository)
    {
        _evenementRepository = evenementRepository;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        List<Evenement> evenements = await _evenementRepository.GetAll();
        return View(evenements);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Show(int id, string? option)
    {
        var evenement = await _evenementRepository.FindById(id);

        if (evenement == null) return NotFound();

        return View(evenement);
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        return Content("Vue Edit de EvenementsController, id: " + id);
    }
}