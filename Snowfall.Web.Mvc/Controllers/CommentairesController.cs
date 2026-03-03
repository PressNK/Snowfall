using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Extensions;
using Snowfall.Web.Mvc.Models.Commentaires;
using Snowfall.Web.Mvc.Models.Questions;

namespace Snowfall.Web.Mvc.Controllers;

[Route("Evenements/{evenementId:int}/[controller]")]
public class CommentairesController : Controller
{
    private readonly ICommentaireService _commentaireService;
    private readonly IEvenementService _evenementService;

    public CommentairesController(ICommentaireService commentaireService, IEvenementService evenementService)
    {
        _commentaireService = commentaireService;
        _evenementService = evenementService;
    }

    [HttpGet] //GET /Evenements/{evenementId}/Questions
    [Authorize]
    public async Task<IActionResult> Index(int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        if (evenement == null) return NotFound();
        
        var commentaires = await _commentaireService.FindByEvenementIdAndUserId(evenementId, User.Identity!.Id());
        
        var viewModel = new CommentairesIndexViewModel()
        {
            Evenement = evenement,
            Commentaires = commentaires
        };
        return View(viewModel);
    }
    
    //GET /Evenements/{evenementId}/Commentaires/New
    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> New(int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        if (evenement == null) return NotFound();
        
        var viewModel = new CreerCommentaireViewModel()
        {
            EvenementId = evenementId
        };
        return View(viewModel);
    }
    
    //POST /Evenements/{evenementId}/Questions
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int evenementId, CreerCommentaireViewModel creerCommentaireViewModel)
    {
        var evenement = await _evenementService.FindById(evenementId);
        if (evenement == null) return NotFound();
        
        if (!ModelState.IsValid)
        {
            return View("New", creerCommentaireViewModel);
        }

        var commentaireToCreate = new Commentaire
        {
            EvenementId = creerCommentaireViewModel.EvenementId,
            Contenu = creerCommentaireViewModel.Contenu,
            UtilisateurId = User.Identity!.Id()
        };
        await _commentaireService.Create(commentaireToCreate);
        
        TempData["success"] = "Nous avons bien reçu votre commentaire!";
        
        // redirect page produit tho
        return RedirectToAction("Show", "Evenements", new { Id = evenementId });
    }
    
    //GET /Evenements/{evenementId}/Commentaires/{id}/Edit
    [HttpGet("{id:int}/[action]")] 
    [Authorize]
    public async Task<IActionResult> Edit(int id, int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        var commentaire = await _commentaireService.FindById(id);
        if (evenement == null || commentaire == null)
            return NotFound();
        if (commentaire.UtilisateurId != User.Identity?.Id())
            return Forbid();
        
        var viewModel = new ModifierCommentaireViewModel()
        {
            Id = commentaire.Id,
            EvenementId = commentaire.EvenementId,
            Contenu = commentaire.Contenu
        };
        
        return View(viewModel);
    }
    
    // PATCH /Evenements/{evenementId}/Commentaires/{id}
    [HttpPatch("{id:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, int evenementId, ModifierCommentaireViewModel modifierCommentaireViewModel)
    {
        var evenement = await _evenementService.FindById(evenementId);
        var commentaire = await _commentaireService.FindById(id);
        if (evenement == null || commentaire == null)
            return NotFound();
        if (commentaire.UtilisateurId != User.Identity?.Id())
            return Forbid();
        
        if (!ModelState.IsValid)
        {
            return View("Edit", modifierCommentaireViewModel);
        }

        commentaire.Contenu = modifierCommentaireViewModel.Contenu;
        
        await _commentaireService.Update(commentaire);
        
        TempData["success"] = "Nous avons bien reçu votre modification à votre commentaire!";
        
        return RedirectToAction("Index", "Commentaires", new { EvenementId = evenementId });
    }
    
    // DELETE /Evenements/{evenementId}/Commentaires/{id}
    [HttpDelete("{id:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        var commentaire = await _commentaireService.FindById(id);
        if (evenement == null || commentaire == null)
            return NotFound();
        if (commentaire.UtilisateurId != User.Identity?.Id())
            return Forbid();
        await _commentaireService.Delete(id);
        TempData["success"] = "Nous avons bien supprimé votre commentaire!";
        return RedirectToAction("Index", "Commentaires", new { EvenementId = evenementId });
    }
    
    // GET /Evenements/{idEvenement}/Commentaires/{id}/Confirmationdelete
    [HttpGet("{id:int}/[action]")]
    [Authorize]
    public async Task<IActionResult> ConfirmationDelete(int evenementId, int id)
    {
        var commentaire = await _commentaireService.FindById(id);

        if (commentaire == null) return NotFound();
    
        return PartialView("_ModalConfirmationDelete", commentaire);
    }
}