using System.ComponentModel.DataAnnotations;

namespace Snowfall.Application.Dtos.Auth;

public class ConnexionDto
{
    // N'oubliez pas la gestion des messages d'erreur (Ressources)
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }   
}