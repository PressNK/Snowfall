using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Dtos.Evenements;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;

namespace Snowfall.Web.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillesController : ControllerBase
{
    private readonly IVilleService _villeService;
    private readonly IMapper _mapper;
        
    public VillesController(
        IVilleService villeService,
        IMapper mapper)
    {
        _villeService = villeService;
        _mapper = mapper;
    }

    // GET /api/villes
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<Ville> villes = await _villeService.GetAll();
        List<VilleDto> villesDto = _mapper.Map<List<VilleDto>>(villes);

        return Ok(villesDto);
    }
}