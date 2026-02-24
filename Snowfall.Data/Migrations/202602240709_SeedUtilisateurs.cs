using System.Data;
using FluentMigrator;
using Microsoft.AspNetCore.Identity;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Migrations;

[Migration(202602240709)]
public class SeedUtilisateurs : Migration
{
    public override void Up()
    {
        string utilisateurGuid = System.Guid.NewGuid().ToString();
        Insert.IntoTable("application_users").Row(new
        {
            id = utilisateurGuid,
            username = "u@ser.com",
            normalized_username = "U@SER.COM",
            email = "u@ser.com",
            normalized_email = "U@SER.com",
            email_confirmed = false,
            password_hash = new PasswordHasher<ApplicationUser>().HashPassword(null, "!User122432"),
            prenom = "Toto",
            nom = "Titi",
        });
        
        // Rôles
        int roleUtilisateurId = 1;

        Insert.IntoTable("application_roles").Row(new
        {
            name = "UTILISATEUR",
            normalized_name = "UTILISATEUR"
        });

        Insert.IntoTable("application_roles_users").Row(new
        {
            role_id = roleUtilisateurId,
            user_id = utilisateurGuid
        });
        
        Insert.IntoTable("informations_client").Row(new
        {
            id = 1,
            user_id = utilisateurGuid,
            ville = "Beloeil",
            code_postal = "HOH OHO",
            province = "Québec",
            pays = "Canada"
        });
    }

    public override void Down()
    {
        Delete.FromTable("application_users").AllRows();
        Delete.FromTable("application_roles").AllRows();
    }
}