using Scalar.AspNetCore;
using Snowfall.Data.Configurations;
using Snowfall.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Injection de dépendances
builder.Services.EnregistrerServices();

builder.Services.AddMigrations(builder.Configuration.GetConnectionString("AppDatabaseConnection")!);

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();