using AngleSharp;
using AngleSharp.Html.Dom;
using Snowfall.Domain.Models;
using Snowfall.Tests.Helpers;

namespace Snowfall.Tests.Web.Mvc.TestsIntegration;

public class AccueilTests : WebMvcIntegrationTestBase
{
    public AccueilTests(
        SnowfallMvcApplicationFactory application, 
        TestDatabaseFixture databaseFixture) : base(application, databaseFixture)
    { }
    
    [Fact]
    public async Task Obtenir_AccueilUtilisateurAnonyme_PageAfficheTexte()
    {
        // Arrange
        string url = "/";
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
        HttpResponseMessage reponse = await Client.GetAsync(url);
    
        // Assert
        reponse.EnsureSuccessStatusCode();
        IHtmlDocument pageAccueil = await HtmlHelpers.GetDocumentAsync(reponse);
        Assert.Contains("Filtrer par ville", pageAccueil.ToHtml());
        Assert.Contains("Détails", pageAccueil.ToHtml());
        Assert.Contains(evenementsList.First().Nom, pageAccueil.ToHtml());
    }
    
    [Fact]
    public async Task Obtenir_AccueilUtilisateurConnecte_PageAfficheNomUtilisateur()
    {
        // Arrange
        string url = "/";
        HttpClient client = await CreerClientUtilisateur(
            "u@ser.com", 
            "!User122432");
    
        // Act
        HttpResponseMessage reponse = await client.GetAsync(url);

        // Assert
        reponse.EnsureSuccessStatusCode();
        string contenu = await reponse.Content.ReadAsStringAsync();
        Assert.Contains("Bonjour,", contenu);
    }
}