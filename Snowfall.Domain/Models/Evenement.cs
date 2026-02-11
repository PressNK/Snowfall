namespace Snowfall.Domain.Models;

public class Evenement
{
    public int Id { get; set; }
    public required string Nom;
    public string? Description;
    public string? ImagePath;
    public DateTime Date;
    public decimal Prix;
    public int Capacite;
    public required int VilleId;
    public required Ville Ville;
}