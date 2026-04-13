using Snowfall.Application.Dtos.Evenements;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Mappings;

public static class EvenementMappingExtensions
{
    public static void ApplyTo(this ModifierEvenementDto src, Evenement dest)
    {
        dest.VilleId = src.VilleId;
        dest.Description = src.Description;
        dest.Nom = src.Nom;
        if (src.ImagePath != null) dest.ImagePath = src.ImagePath;
        dest.Date = src.Date;
        dest.Capacite = src.Capacite;
        dest.Prix = src.Prix;
    }
}