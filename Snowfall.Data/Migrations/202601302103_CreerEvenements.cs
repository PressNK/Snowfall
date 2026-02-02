using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202601302103)]
public class CreerEvenements : Migration
{
    public override void Up ()
    {
        Create.Table("evenements")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("nom").AsString()
            .WithColumn("description").AsString(int.MaxValue)
            .WithColumn("image_path").AsString().Nullable()
            .WithColumn("date").AsDateTime()
            .WithColumn("prix").AsDecimal(10,2)
            .WithColumn("capacite").AsInt32()
            .WithColumn("ville_id").AsInt64().ForeignKey("villes", "id");
    }

    public override void Down()
    {
        Delete.Table("evenements");
    }
}