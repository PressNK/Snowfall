using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Panier;

namespace Snowfall.Web.Mvc.Models.Achats;

public class CheckoutViewModel
{
    public List<PanierItemViewModel> Items { get; set; } = [];
    public ApplicationUser? Utilisateur { get; set; }
    public decimal SousTotal { get; set; }
    public decimal Livraison { get; set; }
    public decimal Total { get; set; }
    public InformationClientModel? InformationClient { get; set; }
}

