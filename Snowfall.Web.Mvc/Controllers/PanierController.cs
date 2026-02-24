using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Web.Mvc.Models.Panier;

namespace Snowfall.Web.Mvc.Controllers;

[Route("[controller]")]
public class PanierController : Controller
{
    private IEvenementService _evenementService;
    
    public PanierController(IEvenementService evenementService)
    {
        _evenementService = evenementService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var panier = HttpContext.Session.GetString("panier");
        if (panier != null)
        {
            var panierItems = JsonSerializer.Deserialize<List<PanierItemViewModel>>(panier);
            if (panierItems != null)
            {
                foreach (var item in panierItems)
                {
                    item.Evenement = await _evenementService.FindById(item.ItemId);
                }
            }
            return View(panierItems);
        }

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(PanierItemViewModel panierItem)
    {
        var evenement = await _evenementService.FindById(panierItem.ItemId);
        if (evenement != null)
        {
            var panier = HttpContext.Session.GetString("panier");
            // si panier est null ou ne peut pas être désérialisé, on initialise une nouvelle liste,
            // sinon on utilise la liste existante
            var panierItems = panier != null
                ? JsonSerializer.Deserialize<List<PanierItemViewModel>>(panier) ?? new List<PanierItemViewModel>()
                : new List<PanierItemViewModel>();
            
            var existingItem = panierItems.FirstOrDefault(i => i.ItemId == panierItem.ItemId);
            if (existingItem != null)
                existingItem.Quantite += panierItem.Quantite;
            else
                panierItems.Add(panierItem);

            HttpContext.Session.SetString("panier", JsonSerializer.Serialize(panierItems));
            return RedirectToAction("Index", "Panier");
        }
        return BadRequest();
    }
    
    [HttpPost("[action]")]
    public IActionResult DeleteItem(int itemId)
    {
        var panier = HttpContext.Session.GetString("panier");
        var panierItems = JsonSerializer.Deserialize<List<PanierItemViewModel>>(panier ?? "[]") ?? new List<PanierItemViewModel>();
        panierItems.RemoveAll(i => i.ItemId == itemId);
        HttpContext.Session.SetString("panier", JsonSerializer.Serialize(panierItems));
        return RedirectToAction("Index", "Panier");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Delete()
    {
        HttpContext.Session.Remove("panier");
        return RedirectToAction("Index", "Panier");
    }
}