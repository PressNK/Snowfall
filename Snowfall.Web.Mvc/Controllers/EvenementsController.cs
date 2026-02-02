using Microsoft.AspNetCore.Mvc;
using Snowfall.Web.Mvc.Models;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class EvenementsController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        List<Evenement> evenements = new List<Evenement>()
        {
            new Evenement()
            {
                Id = 1,
                Nom = "Super Duper Évènement LoL",
                Description = "Compétition de League of Legends.",
                Capacite = 1400, // 1400 personnes
                Date = DateTime.Now + TimeSpan.FromDays(30), // dans 30 jours
                Prix = new decimal(49.00), // 49$ l'inscription, en decimal
                Ville = "Montreal, CA",
            },
            new Evenement()
            {
                Id = 2,
                Nom = "Super Duper Évènement Space Quest 6 Roger Wilco",
                Description = "À quoi ressemblerait une compétition de Space Quest 6 est un mystère.",
                Capacite = 50, // 1400 personnes
                Date = DateTime.Now + TimeSpan.FromDays(60), // dans 60 jours
                Prix = new decimal(29.00), // 29$ l'inscription, en decimal
                Ville = "Los Angeles, US",
                ImagePath = "https://placehold.co/600x400",
            },
        };
        return View(evenements);
    }

    [HttpGet("{id:int}")]
    public IActionResult Show(int id, string? option)
    {
        return Content($"Vue Show de EvenementsController, id: {id}, option: {option}");
    }
    
    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        return Content($"Vue Edit de EvenementsController, id: {id}");
    }
}