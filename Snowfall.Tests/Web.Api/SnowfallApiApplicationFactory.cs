using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Snowfall.Tests.Web.Api;

public class SnowfallApiApplicationFactory : WebApplicationFactory<Snowfall.Web.Api.Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        
        return base.CreateHost(builder);
    }
}