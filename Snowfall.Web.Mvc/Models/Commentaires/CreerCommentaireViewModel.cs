using System.ComponentModel.DataAnnotations;

namespace Snowfall.Web.Mvc.Models.Commentaires;

public class CreerCommentaireViewModel
{
    [Required]
    public int EvenementId { get; set; }
    
    [Required]
    public string Contenu { get; set; } = String.Empty;
}