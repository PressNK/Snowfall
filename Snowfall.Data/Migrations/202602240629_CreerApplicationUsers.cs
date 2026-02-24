using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202602240629)]
public class CreerApplicationUsers : Migration 
{
    public override void Up()
    {
        Create.Table("application_users")
            .WithColumn("id").AsString(255).PrimaryKey()
            .WithDefault(SystemMethods.NewGuid)
            .WithColumn("username").AsString(255)
            .WithColumn("normalized_username").AsString(255)
            .WithColumn("email").AsString(255)
            .WithColumn("normalized_email").AsString(255)
            .WithColumn("email_confirmed").AsBoolean()
            .WithColumn("password_hash").AsString(255)
            .WithColumn("prenom").AsString(255)
            .WithColumn("nom").AsString(255);

        Create.Index("index_application_users_normalized_user_name")
            .OnTable("application_users")
            .OnColumn("normalized_username");
        Create.Index("index_application_users_normalized_email")
            .OnTable("application_users")
            .OnColumn("normalized_username");
    }

    public override void Down()
    {
        Delete.Table("application_users");
    }
}