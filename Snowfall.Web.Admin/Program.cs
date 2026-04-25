using System.Globalization;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Snowfall.Application.Mappings;
using Snowfall.Web.Admin;
using Snowfall.Web.Admin.Configurations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

/* --- Injection de dépendances --- */
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.EnregistrerServices();
// Localization
builder.Services.AddLocalization();
// Blazor Bootstrap
builder.Services.AddBlazorBootstrap();
// Local Storage
builder.Services.AddBlazoredLocalStorage();
// AutoMapper
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<AutoMapperConfig>(); 
});
// Authorization
builder.Services.AddAuthorizationCore();
/* -------------------------------- */

builder.Services.AddLocalization();

var host = builder.Build();

const string defaultCulture = "fr";

var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");
var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

if (result == null)
{
    await js.InvokeVoidAsync("blazorCulture.set", defaultCulture);
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();

//await builder.Build().RunAsync();