using Snowfall.Domain.Models;

namespace Snowfall.Tests.Web.Mvc.TestsE2E;

public class AccueilTests : WebMvcE2ETestBase
{
    public AccueilTests(
        SnowfallMvcKestrelApplicationFactory factory,
        TestDatabaseFixture db)
        : base(factory, db) { }
    
    [Fact]
    public async Task Obtenir_AccueilUtilisateurAnonyme_PageContientEvenements()
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
        };
        foreach (var evenement in evenementsList)
        {
            await EvenementRepository.Create(evenement);       
        }
    
        // Act
        await Page.GotoAsync($"{BaseUrl}");
    
        // Assert
        Assert.Contains(ville.Nom, await Page.ContentAsync());
    }
}