using System.ComponentModel.DataAnnotations;

namespace Snowfall.Web.Mvc.Models.Auth;

public class ConnexionViewModel
{
    [Required(ErrorMessage = "Email_Required")]
    [EmailAddress(ErrorMessage = "Email_Invalid")]
    [Display(Name = "Courriel", Prompt = "Courriel")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password_Required")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password_Length")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe", Prompt = "Mot de passe")]
    public string? Password { get; set; }
}