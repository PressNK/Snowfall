namespace Snowfall.Web.Mvc.Models;

public class Evenement
{
    public int Id { get; set; }
    public required string Nom;
    public string? Description;
    public string? ImagePath;
    public DateTime Date;
    public decimal Prix;
    public int Capacite;
    public required string Ville;
}