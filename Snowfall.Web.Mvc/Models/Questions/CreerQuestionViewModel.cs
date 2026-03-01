using System.ComponentModel.DataAnnotations;

namespace Snowfall.Web.Mvc.Models.Questions;

public class CreerQuestionViewModel
{
    [Required]
    public int EvenementId { get; set; }
    
    [Required]
    public string Contenu { get; set; } = String.Empty;
}