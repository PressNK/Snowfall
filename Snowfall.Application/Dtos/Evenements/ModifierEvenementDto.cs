using System.ComponentModel.DataAnnotations;

namespace Snowfall.Application.Dtos.Evenements;

public class ModifierEvenementDto
{
    [Required(ErrorMessageResourceType = typeof(Resources.Evenements), ErrorMessageResourceName = "Nom_Required")]
    public string Nom { get; set; } = null!;
    [MinLength(10, ErrorMessageResourceType = typeof(Resources.Evenements), ErrorMessageResourceName = "Description_MinLength")]
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Evenements), ErrorMessageResourceName = "Date_Required")]
    public DateTime Date { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Evenements), ErrorMessageResourceName = "Prix_Required")]
    public Decimal Prix { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Evenements), ErrorMessageResourceName = "Capacite_Required")]
    public int Capacite { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources.Evenements), ErrorMessageResourceName = "VilleId_Required")]
    public int VilleId { get; set; }
}