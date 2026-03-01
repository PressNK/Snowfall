namespace Snowfall.Domain.Models;

public class Question
{
    public int Id { get; set; }
    public int EvenementId { get; set; }
    public required string UtilisateurId { get; set; }
    public ApplicationUser? Utilisateur { get; set; }
    public required string Contenu { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}