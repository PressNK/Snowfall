namespace Snowfall.Domain.Models;

public class Achat
{
    public int Id { get; set; }
    public required string UtilisateurId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal SousTotal { get; set; }
    public decimal Livraison { get; set; }
    public decimal Total { get; set; }
    public ApplicationUser? Utilisateur { get; set; }
    public List<LigneAchat>? LignesAchat { get; set; }
    public string? StatutPaiement { get; set; }
    public string? StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }
}

