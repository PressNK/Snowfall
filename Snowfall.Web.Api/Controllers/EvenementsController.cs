using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Dtos.Evenements;
using Snowfall.Application.Mappings;
using Snowfall.Application.Services;
using Snowfall.Domain.Models;

namespace Snowfall.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvenementsController : ControllerBase
    {
        private readonly IEvenementService _evenementService;
        private readonly IMapper _mapper;
    
        public EvenementsController(IEvenementService evenementService, IMapper mapper)
        {
            _evenementService = evenementService;
            _mapper = mapper;
        }
        
        // GET /api/evenements
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Evenement> evenements = await _evenementService.GetAll();
        
            List<EvenementDto> evenementDtos = _mapper.Map<List<EvenementDto>>(evenements);

            return Ok(evenementDtos);
        }
        
        // GET /api/evenements/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Show(int id)
        {
            var evenement = await _evenementService.FindById(id);

            if (evenement is null) return NotFound();

            return Ok(_mapper.Map<EvenementDto>(evenement));
        }
        
        /// <summary>
        /// POST /api/evenements
        /// Permets de créer un événement.
        /// </summary>
        /// <param name="creerEvenementDto">Le DTO d'événement contenant les informations de l'événement à créer</param>
        /// <returns>L'événement créé</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreerEvenementDto creerEvenementDto)
        {
            Evenement evenement = _mapper.Map<Evenement>(creerEvenementDto);
            evenement = await _evenementService.Create(evenement);
            return Ok(_mapper.Map<EvenementDto>(evenement));
        }
        
        /// <summary>
        /// PATCH /api/evenements/{id}
        /// Permets de modifier un événement
        /// </summary>
        /// <param name="id">Le id de l'événement à modifier</param>
        /// <param name="modifierEvenementDto">Le DTO de l'événement modifié à sauvegarder</param>
        /// <returns></returns>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, ModifierEvenementDto modifierEvenementDto)
        {
            var evenement = await _evenementService.FindById(id);
            if (evenement == null)
                return NotFound();
            
            modifierEvenementDto.ApplyTo(evenement);
            
            bool updated = await _evenementService.Update(evenement);
            if (!updated)
                return UnprocessableEntity();
            
            return Ok(_mapper.Map<EvenementDto>(evenement));
        }
        
    }
}
