namespace Snowfall.Domain.Models;

public class ApplicationUser
{
    public string? Id { get; set; }
    public required string UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public required string Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PasswordHash { get; set; }

    #region Propriétés personnalisées
    public required string Prenom { get; set; }
    public required string Nom { get; set; }
    #endregion
}