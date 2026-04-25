namespace Snowfall.Domain.Models;

public class LigneAchat
{
    public int Id { get; set; }
    public int AchatId { get; set; }
    public int EvenementId { get; set; }
    public int Quantite { get; set; }
    public decimal PrixUnitaire { get; set; }
    public string? EvenementNom { get; set; }
    public Evenement? Evenement { get; set; }
}

