using Snowfall.Domain.Models;
using Snowfall.Web.Mvc.Models.Panier;

namespace Snowfall.Web.Mvc.Models.Achats;

public class ConfirmationViewModel
{
    public Achat Achat { get; set; } = null!;
    public InformationClientModel InformationClient { get; set; } = null!;
}

