using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Models.Evenements;

public class EvenementsIndexViewModel
{
    public required List<Evenement> Evenements { get; set; }
    public required FiltresEvenementsViewModel FiltresEvenements { get; set; }
}