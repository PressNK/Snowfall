using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202602240632)]
public class CreerApplicationRoles : Migration
{
    public override void Up()
    {
        Create.Table("application_roles")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("name").AsString(255)
            .WithColumn("normalized_name").AsString(255);

        Create.Index("index_application_roles_normalized_name")
            .OnTable("application_roles")
            .OnColumn("normalized_name");
    }

    public override void Down()
    {
        Delete.Table("application_roles");
    }
}