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
    
    [Required(ErrorMessage = "Adresse_Required")]
    [Display(Name = "Adresse", Prompt = "Adresse")]
    public required string Adresse { get; set; }

    [Required(ErrorMessage = "Ville_Required")]
    [Display(Name = "Ville", Prompt = "Ville")]
    public required string Ville { get; set; }

    [Required(ErrorMessage = "CodePostal_Required")]
    [Display(Name = "Code postal", Prompt = "Code postal")]
    public required string CodePostal { get; set; }

    [Required(ErrorMessage = "Province_Required")]
    [Display(Name = "Province", Prompt = "Province")]
    public required string Province { get; set; }

    [Required(ErrorMessage = "Pays_Required")]
    [Display(Name = "Pays", Prompt = "Pays")]
    public required string Pays { get; set; }
    
    [Required(ErrorMessage = "Password_Required")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "Password_Length")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe", Prompt = "Mot de passe")]
    public string Password { get; set; } = String.Empty;
    
    [Required(ErrorMessage = "Password_Required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords_NoMatch")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmer mot de passe", Prompt = "Confirmer mot de passe")]
    public string ConfirmPassword { get; set; } = String.Empty;
}