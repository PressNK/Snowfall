using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Snowfall.Web.Mvc;

namespace Snowfall.Tests.Web.Mvc;

public class SnowfallMvcApplicationFactory : WebApplicationFactory<Snowfall.Web.Mvc.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
    }
}