using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Models.Commentaires;

public class CommentairesIndexViewModel
{
    public required Evenement Evenement { get; set; }
    public required List<Commentaire> Commentaires { get; set; }
}