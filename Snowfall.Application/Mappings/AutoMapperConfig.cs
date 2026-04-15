using AutoMapper;
using Snowfall.Application.Dtos.Evenements;
using Snowfall.Domain.Models;

namespace Snowfall.Application.Mappings;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Evenement, EvenementDto>().ReverseMap();
        CreateMap<CreerEvenementDto, Evenement>();
        CreateMap<EvenementDto, ModifierEvenementDto>();
        CreateMap<Ville, VilleDto>().ReverseMap();
    }
}