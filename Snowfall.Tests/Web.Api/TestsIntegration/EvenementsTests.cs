using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Snowfall.Application.Dtos.Evenements;
using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

namespace Snowfall.Tests.Web.Api.TestsIntegration;

public class EvenementsTests : WebApiIntegrationTestBase
{

    public EvenementsTests(
        SnowfallApiApplicationFactory application,
        TestDatabaseFixture database) : base(application, database)
    { }
    
    [Fact]
    public async Task Obtenir_ListeEvenements_RetourneListeSucces()
    {
        // Arrange
        var ville = new Ville()
        {
            Nom = "Paris",
            PaysIso = "fr"
        };
        ville = await VilleRepository.Create(ville);

        List<Evenement> evenementsList = new()
        {
            new Evenement()
            {
                Nom = "Evenement de test",
                Description = "Description de l'événement",
                Capacite = 10,
                Prix = 100,
                Date = DateTime.Now,
                ImagePath = "image.jpg",
                VilleId = ville.Id,
            },
            new Evenement()
            {
                Nom = "Autre evenement de test",
                Description = "Description de l'événement",
                Capacite = 100,
                Prix = 200,
                Date = DateTime.Now,
                ImagePath = "image.jpg",
                VilleId = ville.Id,
            }
        };
        foreach (var evenement in evenementsList)
        {
            await EvenementRepository.Create(evenement);       
        }
        
        string url = "api/evenements";
    
        // Act
        var evenements = await Client.GetFromJsonAsync<List<EvenementDto>?>(url);

        // Assert
        Assert.NotNull(evenements);
        Assert.NotEmpty(evenements);
    }
}