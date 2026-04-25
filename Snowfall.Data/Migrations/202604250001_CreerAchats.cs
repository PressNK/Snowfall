using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202604250001)]
public class CreerAchats : Migration
{
    public override void Up()
    {
        Create.Table("achats")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("utilisateur_id").AsString(255).ForeignKey("application_users", "id")
            .WithColumn("sous_total").AsDecimal(10, 2)
            .WithColumn("livraison").AsDecimal(10, 2)
            .WithColumn("total").AsDecimal(10, 2)
            .WithColumn("created_at").AsDateTime();

        Create.Table("lignes_achat")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("achat_id").AsInt64().ForeignKey("achats", "id")
            .WithColumn("evenement_id").AsInt64().ForeignKey("evenements", "id")
            .WithColumn("quantite").AsInt32()
            .WithColumn("prix_unitaire").AsDecimal(10, 2);
    }

    public override void Down()
    {
        Delete.Table("lignes_achat");
        Delete.Table("achats");
    }
}

