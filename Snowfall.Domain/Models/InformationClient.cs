namespace Snowfall.Domain.Models;

public class InformationClient
{
    public int Id { get; set; }
    public string? UtilisateurId { get; set; }
    public required string Adresse { get; set; }
    public required string Ville { get; set; }
    public required string CodePostal { get; set; }
    public required string Province { get; set; }
    public required string Pays { get; set; }
}