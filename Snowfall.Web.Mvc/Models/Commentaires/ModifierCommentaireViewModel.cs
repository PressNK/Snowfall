using System.ComponentModel.DataAnnotations;

namespace Snowfall.Web.Mvc.Models.Commentaires;

public class ModifierCommentaireViewModel : CreerCommentaireViewModel
{
    [Required]
    public int Id { get; set; }
}