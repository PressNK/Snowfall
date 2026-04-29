using System.Net.Http.Json;
using Snowfall.Application.Dtos.Auth;

namespace Snowfall.Tests.Web.Api.TestsE2E;

public class AccueilTest : WebApiE2ETestBase
{
    public AccueilTest(
        SnowfallApiKestrelApplicationFactory factory,
        TestDatabaseFixture db)
        : base(factory, db)
    {
    }

    [Fact]
    public async Task Obtenir_PageAccueil()
    {
        // Arrange
        // Effectuer un appel de connexion à l'API pour obtenir un jeton
        // Arrange
        await AuthentifierUtilisateur("u@admin.com", "admin");
        
        // Act
        await Page.GotoAsync($"{BaseUrl}");
        
        await Page.Locator("h1").WaitForAsync();
        
        // Assert
        Assert.Contains("Gestion Snowfall", await Page.ContentAsync());
    }

}