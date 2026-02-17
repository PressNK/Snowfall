namespace Snowfall.Domain.Models;

public class Evenement
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public DateTime Date { get; set; }
    public decimal Prix { get; set; }
    public int Capacite { get; set; }
    public required int VilleId { get; set; }
    public required Ville Ville { get; set; }
}