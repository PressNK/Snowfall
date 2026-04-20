using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snowfall.Application.Dtos.Uploads;

namespace Snowfall.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public UploadsController(IWebHostEnvironment environment, 
            IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile file)
        {
            // Nom de fichier source tel que reçu du client
            string nomFichierSource = file.FileName;
    
            // La taille maximale de fichier acceptée (5 Mo)
            long tailleFichierMax = 1024 * 1024 * 5; // 5mb
    
            // L'URL de base qui sera utilisée pour retourner l'URL du fichier
            var baseResourceUrl = new Uri($"{Request.Scheme}://{Request.Host}");
            
            var resultatUpload = new ResultatUpload()
            {
                EstSucces = false,
            };
            
            if (file.Length == 0)
                resultatUpload.Erreur = ErreurUpload.FichierVide;
            else if (file.Length > tailleFichierMax)
                resultatUpload.Erreur = ErreurUpload.TailleMax;
    
            if(resultatUpload.Erreur is not null)
                return BadRequest(resultatUpload);
            
            try
            {
                // On récupère l'extension du fichier
                string extension = Path.GetExtension(nomFichierSource);
        
                // On crée un nom de fichier unique à l'aide d'un GUID pour éviter
                // les collisions si on soumettait plusieurs fois le même nom de fichier
                string nomFichier = Guid.NewGuid().ToString();
        
                // Le nom de fichier final complet est créé
                string nomFichierStorage = nomFichier + extension;
        
                // Le chemin de sauvegarde est le dossier Storage
                string path = Path.Combine(_environment.ContentRootPath,
                    _configuration["DossierStorage"]!,
                    nomFichierStorage);

                // Écriture du fichier sur le disque
                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);
        
                // Compléter le DTO
                resultatUpload.EstSucces = true;
                resultatUpload.NomFichier = nomFichierStorage;
                resultatUpload.UrlFichier = $"{baseResourceUrl}storage/{nomFichierStorage}";
            }
            catch (IOException ex)
            {
                resultatUpload.Erreur = ErreurUpload.Ecriture;
                return BadRequest(resultatUpload);
            }

            // Retour 201 Created avec le DTO
            return Created(resultatUpload.UrlFichier!, resultatUpload);
        }
    }
}