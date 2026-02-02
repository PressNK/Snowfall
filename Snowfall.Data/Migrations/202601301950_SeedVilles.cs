using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202601301950)]
public class SeedVilles : Migration
{
    public override void Up()
    {
        Insert.IntoTable("villes").Row(new
        {
            nom = "Vegas, NV",
            pays_iso = "US"
        });
        
        Insert.IntoTable("villes").Row(new
        {
            nom = "Montreal, QC",
            pays_iso = "CA"
        });
    }

    public override void Down()
    {
        Delete.FromTable("villes").AllRows();
    }
}