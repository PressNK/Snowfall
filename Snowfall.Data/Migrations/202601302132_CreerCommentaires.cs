using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202601302132)]
public class CreerCommentaires : Migration
{
    public override void Up ()
    {
        Create.Table("commentaires")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("evenement_id").AsInt64().ForeignKey("evenements", "id")
            .WithColumn("contenu").AsString(int.MaxValue)
            .WithColumn("created_at").AsDateTime()
            .WithColumn("updated_at").AsDateTime();
    }

    public override void Down()
    {
        Delete.Table("commentaires");
    }
}