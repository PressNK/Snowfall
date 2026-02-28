using Microsoft.AspNetCore.Identity;
using Snowfall.Application.Claims;
using Snowfall.Application.Services;
using Snowfall.Data.Configurations;
using Snowfall.Data.Context;
using Snowfall.Data.Repositories;
using Snowfall.Domain.Models;

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
builder.Services.AddScoped<IEvenementRepository, EvenementRepository>();
builder.Services.AddScoped<IVilleRepository, VilleRepository>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IEvenementService, EvenementService>();
builder.Services.AddScoped<IVilleService, VilleService>();
builder.Services.AddScoped<IRoleStore<ApplicationRole>, RoleRepository>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, UserRepository>();

// Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddClaimsPrincipalFactory<ApplicationClaimsPrincipalFactory>();;

// Dapper match underscores: nom_propriete_underscore <-> NomProprieteUnderscore
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

app.Services.MigrateDown();
app.Services.MigrateUp();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Evenements}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();