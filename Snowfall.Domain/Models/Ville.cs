namespace Snowfall.Domain.Models;

public class Ville
{
    public int Id { get; set; }
    public required string Nom { get; set; }
    public required string PaysIso { get; set; }
}