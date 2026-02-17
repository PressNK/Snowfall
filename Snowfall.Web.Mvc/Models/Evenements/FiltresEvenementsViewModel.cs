using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Models.Evenements;

public class FiltresEvenementsViewModel
{
    public required List<Ville> Villes { get; set; }
    public int? SelectedVilleId { get; set; }
}