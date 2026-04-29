using Snowfall.Tests.Web.Mvc.TestsIntegration;

namespace Snowfall.Tests.Web.Mvc;

public class SnowfallMvcApplicationServeurTest : WebMvcIntegrationTestBase
{
    public SnowfallMvcApplicationServeurTest(
        SnowfallMvcApplicationFactory application,
        TestDatabaseFixture database) : base(application, database)
    {
    }
}