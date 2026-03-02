using System.ComponentModel.DataAnnotations;

namespace Snowfall.Web.Mvc.Models.Comptes;

public class InscriptionViewModel
{
    [Required(ErrorMessage = "Prenom_Required")]
    [Display(Name = "Prenom", Prompt = "Prenom")]
    public string Prenom { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Nom_Required")]
    [Display(Name = "Nom", Prompt = "Nom")]
    public string Nom { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Email_Required")]
    [EmailAddress(ErrorMessage = "Email_Invalid")]
    [Display(Name = "Courriel", Prompt = "Courriel")]
    public string Email { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Password_Required")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password_Length")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe", Prompt = "Mot de passe")]
    public string Password { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Password_Required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords_NoMatch")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe", Prompt = "Mot de passe")]
    public string ConfirmPassword { get; set; } = String.Empty;
}