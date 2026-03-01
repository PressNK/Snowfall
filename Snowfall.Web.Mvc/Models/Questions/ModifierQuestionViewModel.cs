using System.ComponentModel.DataAnnotations;

namespace Snowfall.Web.Mvc.Models.Questions;

public class ModifierQuestionViewModel : CreerQuestionViewModel
{
    [Required]
    public int Id { get; set; }
}