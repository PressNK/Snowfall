using AutoMapper;
using Snowfall.Application.Dtos.Evenements;
using Snowfall.Domain.Models;

namespace Snowfall.Web.Api.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Evenement, EvenementDto>().ReverseMap();
        CreateMap<CreerEvenementDto, Evenement>();
        CreateMap<Ville, VilleDto>().ReverseMap();
    }
}