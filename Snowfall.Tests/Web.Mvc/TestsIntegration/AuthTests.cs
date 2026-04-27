using System.Net;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using Snowfall.Tests.Helpers;

namespace Snowfall.Tests.Web.Mvc.TestsIntegration;

public class AuthTests : WebMvcIntegrationTestBase
{
    public AuthTests(
        SnowfallMvcApplicationFactory application, 
        TestDatabaseFixture database) : base(application, database)
    {
    }
    
    [Fact]
    public async Task Connexion_AuthentificationSucces_RetourneRedirectionAccueil()
    {
        // Arrange
        var client = Application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        
        HttpResponseMessage reponse = await client.GetAsync("Auth/Connexion");
        reponse.EnsureSuccessStatusCode();
        
        var connexionViewModelDictionary = ConnexionViewModelDictionary();
    
        // Act
        IHtmlDocument pageConnexion = await HtmlHelpers.GetDocumentAsync(reponse);
        reponse = await client.SendAsync(
            (pageConnexion.QuerySelector("main form") as IHtmlFormElement)!,
            (pageConnexion.QuerySelector("main form button.btn-primary") as IHtmlButtonElement)!,
            connexionViewModelDictionary);
        
        // Assert
        Assert.Equal(HttpStatusCode.Redirect, reponse.StatusCode);
        Assert.Equal("/",reponse.Headers.Location?.OriginalString);
        
        reponse = await client.GetAsync(reponse.Headers.Location!.ToString());
        Assert.Contains("Bonjour, Toto", await reponse.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task Connexion_AuthentificationInvalide_RetourneErreursValidation()
    {
        // Arrange
        var client = Application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        HttpResponseMessage reponse = await client.GetAsync("Auth/Connexion");
        reponse.EnsureSuccessStatusCode();

        var connexionViewModelDictionary = ConnexionViewModelDictionary();
        connexionViewModelDictionary["Password"] = String.Empty;

        // Act
        IHtmlDocument pageConnexion = await HtmlHelpers.GetDocumentAsync(reponse);
        reponse = await client.SendAsync(
            (pageConnexion.QuerySelector("main form") as IHtmlFormElement)!,
            (pageConnexion.QuerySelector("main form button.btn-primary") as IHtmlButtonElement)!,
            connexionViewModelDictionary);

        // Assert
        var pageConnexionAvecErreurs = await HtmlHelpers.GetDocumentAsync(reponse);
        var elementsErreurValidation = pageConnexionAvecErreurs.QuerySelectorAll(".input-validation-error");
        Assert.NotNull(elementsErreurValidation);
        Assert.True(elementsErreurValidation.Length > 0);
    }
    
    [Fact]
    public async Task Connexion_MauvaisCourrielMotDePasse_RetourneErreurAlertGenerique()
    {
        // Arrange
        var client = Application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        HttpResponseMessage reponse = await client.GetAsync("Auth/Connexion");
        reponse.EnsureSuccessStatusCode();

        var connexionViewModelDictionary = ConnexionViewModelDictionary();
        connexionViewModelDictionary["Password"] = "Mauvais mdp!";

        // Act
        IHtmlDocument pageConnexion = await HtmlHelpers.GetDocumentAsync(reponse);
        reponse = await client.SendAsync(
            (pageConnexion.QuerySelector("main form") as IHtmlFormElement)!,
            (pageConnexion.QuerySelector("main form button.btn-primary") as IHtmlButtonElement)!,
            connexionViewModelDictionary);

        // Assert
        var pageConnexionAvecErreurs = await HtmlHelpers.GetDocumentAsync(reponse);
        var elementAlert = pageConnexionAvecErreurs.QuerySelector(".alert.alert-danger");
        Assert.NotNull(elementAlert);
        Assert.Contains(
            "La combinaison de courriel et mot de passe est invalide ou il est impossible de vous identifier",
            elementAlert.TextContent);
    }
    
    private Dictionary<string, string> ConnexionViewModelDictionary()
    {
        return new Dictionary<string, string>
        {
            { "Email", "u@ser.com" },
            { "Password", "!User122432" },
        };
    }
    
}