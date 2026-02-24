using System.Data;
using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202602240634)]
public class CreerInformationsClient : Migration
{
    public override void Up()
    {
        Create.Table("informations_client")
            .WithColumn("id").AsInt64().PrimaryKey()
            .WithColumn("user_id").AsString(255)
            .WithColumn("ville").AsString(255)
            .WithColumn("code_postal").AsString(10)
            .WithColumn("province").AsString(255)
            .WithColumn("pays").AsString(255);

        Create.ForeignKey()
            .FromTable("informations_client").ForeignColumn("user_id")
            .ToTable("application_users").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.ForeignKey()
            .FromTable("informations_client").ForeignColumn("user_id")
            .ToTable("application_users").PrimaryColumn("id");
        
        Delete.Table("informations_client");
    }
}