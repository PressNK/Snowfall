using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202603021944)]
public class CreerCommentaires : Migration
{
    public override void Up ()
    {
        Create.Table("commentaires")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("utilisateur_id").AsString(255).ForeignKey("application_users", "id")
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