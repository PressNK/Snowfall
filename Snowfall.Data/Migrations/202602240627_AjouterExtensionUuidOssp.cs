using FluentMigrator;

namespace Snowfall.Data.Migrations;

[Migration(202602240627)]
public class AjouterExtensionUuidOssp : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
            """
        );
    }

    public override void Down()
    {
        Execute.Sql(
            """
            DROP EXTENSION IF EXISTS "uuid-ossp"
            """
        );
    }
}