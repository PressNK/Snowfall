namespace Snowfall.Application.Dtos.Evenements;

public class VilleDto
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public required string PaysIso { get; set; }
}