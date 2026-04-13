namespace Snowfall.Application.Dtos.Evenements;

public class EvenementDto
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public DateTime Date { get; set; }
    public Decimal Prix { get; set; }
    public int Capacite { get; set; }
    public VilleDto? Ville { get; set; }
}