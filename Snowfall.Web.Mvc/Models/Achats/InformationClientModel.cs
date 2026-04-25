namespace Snowfall.Web.Mvc.Models.Achats;

public class InformationClientModel
{
    public required string Adresse { get; set; }
    public required string Ville { get; set; }
    public required string CodePostal { get; set; }
    public required string Province { get; set; }
    public required string Pays { get; set; }
}