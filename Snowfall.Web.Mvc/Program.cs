using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Snowfall.Application.Claims;
using Snowfall.Application.Services;
using Snowfall.Data.Configurations;
using Snowfall.Data.Context;
using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;
using Snowfall.Shared;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddMvcLocalization();

// Session
builder.Services.AddSession();

// Ajoute les migrations
builder.Services.AddMigrations(configuration.GetConnectionString("AppDatabaseConnection")!);

// Injection de dépendances
builder.Services.EnregistrerServices();
builder.Services.AddScoped<IRoleStore<ApplicationRole>, RoleRepository>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, UserRepository>();

// Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddClaimsPrincipalFactory<ApplicationClaimsPrincipalFactory>();;

// Dapper match underscores: nom_propriete_underscore <-> NomProprieteUnderscore
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();


app.Services.MigrateUp();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// permets les méthodes PUT/PATCH/DELETE dans les formulaires. À mettre avant UseRouting()!
app.UseHttpMethodOverride(new() { FormFieldName = "_method" });
app.UseRouting();

var supportedCultures = new[] { "fr", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("fr")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

localizationOptions.RequestCultureProviders.Clear();
app.UseRequestLocalization(localizationOptions);

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Evenements}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["DossierStorage"]!))),
    RequestPath = "/storage"
});


app.Run();