using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202601302104)]
public class SeedEvenements : Migration
{
    private enum Villes
    {
        Vegas = 1,
        Montreal = 2,
    }
    
    public override void Up()
    {
        Insert.IntoTable("evenements").Row(new
        {
            nom = "Compétition de LoL",
            description = "Une super compétition de League of Legends",
            prix = 30.00,
            capacite = 4000,
            date = new DateTime(2025, 2, 7),
            ville_id = (int) Villes.Vegas,
        })
        .Row(new
        {
            nom = "Compétition de Counter-Strike",
            description = "Les meilleurs joueurs de Counter-Strike s'affrontent",
            prix = 30.00,
            capacite = 1000,
            date = new DateTime(2026, 1, 7),
            ville_id = (int) Villes.Montreal,
        });
    }

    public override void Down()
    {
        Delete.FromTable("evenements").AllRows();
    }
}