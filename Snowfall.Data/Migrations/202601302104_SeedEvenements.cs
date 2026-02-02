using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202601302104)]
public class SeedEvenements : Migration
{
    private enum Villes
    {
        Vegas = 1,
    }
    
    public override void Up()
    {
        Insert.IntoTable("evenements").Row(new
        {
            nom = "What happens in Vegas",
            description = "Stays in Vegas",
            prix = 30.00,
            capacite = 4000,
            date = DateTime.Now,
            ville_id = (int) Villes.Vegas,
        });
    }

    public override void Down()
    {
        Delete.FromTable("evenements").AllRows();
    }
}