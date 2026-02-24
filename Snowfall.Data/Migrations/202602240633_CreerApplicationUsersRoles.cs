using System.Data;
using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202602240633)]
public class CreerApplicationUsersRoles : Migration
{
    public override void Up()
    {
        Create.Table("application_roles_users")
            .WithColumn("role_id").AsInt64().PrimaryKey()
            .WithColumn("user_id").AsString(255).PrimaryKey();

        Create.ForeignKey()
            .FromTable("application_roles_users").ForeignColumn("role_id")
            .ToTable("application_roles").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey()
            .FromTable("application_roles_users").ForeignColumn("user_id")
            .ToTable("application_users").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.ForeignKey()
            .FromTable("application_roles_users").ForeignColumn("user_id")
            .ToTable("application_users").PrimaryColumn("id");
        Delete.ForeignKey()
            .FromTable("application_roles_users").ForeignColumn("role_id")
            .ToTable("application_roles").PrimaryColumn("id");
        
        Delete.Table("application_roles_users");

    }
}