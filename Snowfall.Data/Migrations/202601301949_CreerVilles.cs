using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202601301949)]
public class CreerVilles : Migration
{
    public override void Up ()
    {
        Create.Table("villes")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("nom").AsString(255)
            .WithColumn("pays_iso").AsString(2);
    }

    public override void Down()
    {
        Delete.Table("villes");
    }
}