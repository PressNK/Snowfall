using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Extensions;
using Snowfall.Web.Mvc.Models.Questions;

namespace Snowfall.Web.Mvc.Controllers;

[Route("Evenements/{evenementId:int}/[controller]")]
public class QuestionsController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly IEvenementService _evenementService;

    public QuestionsController(IQuestionService questionService, IEvenementService evenementService)
    {
        _questionService = questionService;
        _evenementService = evenementService;
    }

    [HttpGet] //GET /Evenements/{evenementId}/Questions
    [Authorize]
    public async Task<IActionResult> Index(int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        if (evenement == null) return NotFound();
        
        var questions = await _questionService.FindByEvenementIdAndUserId(evenementId, User.Identity!.Id());
        
        var viewModel = new QuestionsIndexViewModel()
        {
            Evenement = evenement,
            Questions = questions
        };
        return View(viewModel);
    }
    
    //GET /Evenements/{evenementId}/Questions/New
    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> New(int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        if (evenement == null) return NotFound();
        
        var viewModel = new CreerQuestionViewModel()
        {
            EvenementId = evenementId
        };
        return View(viewModel);
    }
    
    //POST /Evenements/{evenementId}/Questions
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int evenementId, CreerQuestionViewModel questionViewModel)
    {
        var evenement = await _evenementService.FindById(evenementId);
        if (evenement == null) return NotFound();
        
        if (!ModelState.IsValid)
        {
            return View("New", questionViewModel);
        }

        var questionToCreate = new Question()
        {
            EvenementId = questionViewModel.EvenementId,
            Contenu = questionViewModel.Contenu,
            UtilisateurId = User.Identity!.Id()
        };
        await _questionService.Create(questionToCreate);
        
        TempData["success"] = "Nous avons bien reçu votre question!";
        
        return RedirectToAction("Show", "Evenements", new { Id = evenementId });
    }
    
    //GET /Evenements/{evenementId}/Questions/{id}/Edit
    [HttpGet("{id:int}/[action]")] 
    [Authorize]
    public async Task<IActionResult> Edit(int id, int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        var question = await _questionService.FindById(id);
        if (evenement == null || question == null)
            return NotFound();
        if (question.UtilisateurId != User.Identity?.Id())
            return Forbid();
        
        var viewModel = new ModifierQuestionViewModel()
        {
            Id = question.Id,
            EvenementId = question.EvenementId,
            Contenu = question.Contenu
        };
        
        return View(viewModel);
    }
    
    // PATCH /Evenements/{evenementId}/Questions/{id}
    [HttpPatch("{id:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, int evenementId, ModifierQuestionViewModel modifierQuestionViewModel)
    {
        var evenement = await _evenementService.FindById(evenementId);
        var question = await _questionService.FindById(id);
        if (evenement == null || question == null)
            return NotFound();
        if (question.UtilisateurId != User.Identity?.Id())
            return Forbid();
        
        if (!ModelState.IsValid)
        {
            return View("Edit", modifierQuestionViewModel);
        }

        question.Contenu = modifierQuestionViewModel.Contenu;
        
        await _questionService.Update(question);
        
        TempData["success"] = "Nous avons bien reçu votre modification à votre question!";
        
        return RedirectToAction("Index", "Questions", new { EvenementId = evenementId });
    }
    
    // DELETE /Evenements/{evenementId}/Questions/{id}
    [HttpDelete("{id:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int evenementId)
    {
        var evenement = await _evenementService.FindById(evenementId);
        var question = await _questionService.FindById(id);
        if (evenement == null || question == null)
            return NotFound();
        if (question.UtilisateurId != User.Identity?.Id())
            return Forbid();
        await _questionService.Delete(id);
        TempData["success"] = "Nous avons bien supprimé votre question!";
        return RedirectToAction("Index", "Questions", new { EvenementId = evenementId });
    }
    
    // GET /Evenements/{idEvenement}/Questions/{id}/Confirmationdelete
    [HttpGet("{id:int}/[action]")]
    [Authorize]
    public async Task<IActionResult> ConfirmationDelete(int evenementId, int id)
    {
        var question = await _questionService.FindById(id);

        if (question == null) return NotFound();
    
        return PartialView("_ModalConfirmationDelete", question);
    }
}