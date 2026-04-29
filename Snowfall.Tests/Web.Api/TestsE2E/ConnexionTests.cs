namespace Snowfall.Tests.Web.Api.TestsE2E;

public class ConnexionTests : WebApiE2ETestBase
{
    public ConnexionTests(
        SnowfallApiKestrelApplicationFactory factory,
        TestDatabaseFixture db)
        : base(factory, db) { }

    [Fact]
    public async Task Obtenir_PageConnexion()
    {
        // Arrange
        
        // Act
        await Page.GotoAsync($"{BaseUrl}/connexion");
        await Page.Locator("text=Connexion").WaitForAsync();
        
        // Assert
        var heading = Page.Locator("h1");
        Assert.Contains("Connexion", await heading.TextContentAsync());
    }
}