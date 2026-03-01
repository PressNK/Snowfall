using System.Data;
using FluentMigrator;
using Microsoft.AspNetCore.Identity;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Migrations;

[Migration(202602280940)]
public class CreerQuestions : Migration
{
    public override void Up()
    {
        Create.Table("questions")
            .WithColumn("id").AsInt64().PrimaryKey().Identity()
            .WithColumn("utilisateur_id").AsString(255).ForeignKey("application_users", "id")
            .WithColumn("evenement_id").AsInt64().ForeignKey("evenements", "id")
            .WithColumn("contenu").AsString(int.MaxValue)
            .WithColumn("created_at").AsDateTime()
            .WithColumn("updated_at").AsDateTime();
        
        Create.Index("index_questions_utilisateur_id")
            .OnTable("questions")
            .OnColumn("utilisateur_id");
        
        Create.Index("index_questions_evenement_id")
            .OnTable("questions")
            .OnColumn("utilisateur_id");
    }

    public override void Down()
    {
        Delete.Table("questions");
    }
}