using Scalar.AspNetCore;
using Snowfall.Data.Configurations;
using Snowfall.Shared;
using Snowfall.Web.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

/* -- Injection de dépendances --  */

// Services & Repositories
builder.Services.EnregistrerServices();
// FluentMigration
builder.Services.AddMigrations(builder.Configuration.GetConnectionString("AppDatabaseConnection")!);
// AutoMapper
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<AutoMapperConfig>(); 
});

/* --------------------------------- */

// Dapper match underscores: nom_propriete_underscore <-> NomProprieteUnderscore
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

//app.Services.MigrateDown();
app.Services.MigrateUp();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Blazor
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();