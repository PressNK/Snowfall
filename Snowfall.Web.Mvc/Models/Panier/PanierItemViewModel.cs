using Snowfall.Domain.Models;

namespace Snowfall.Web.Mvc.Models.Panier;

public class PanierItemViewModel
{
    public int ItemId { get; set; }
    public int Quantite { get; set; }
    public Evenement? Evenement { get; set; }
}