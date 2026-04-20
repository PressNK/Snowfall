using System.Data;
using FluentMigrator;
using Microsoft.AspNetCore.Identity;
using Snowfall.Domain.Models;

namespace Snowfall.Data.Migrations;

[Migration(202604200616)]
public class SeedUtilisateursAdmin : Migration
{
    public override void Up()
    {
        string utilisateurGuid = System.Guid.NewGuid().ToString();
        Insert.IntoTable("application_users").Row(new
        {
            id = utilisateurGuid,
            username = "u@admin.com",
            normalized_username = "U@ADMIN.COM",
            email = "u@admin.com",
            normalized_email = "U@ADMIN.com",
            email_confirmed = false,
            password_hash = new PasswordHasher<ApplicationUser>().HashPassword(null, "admin"),
            prenom = "Presley",
            nom = "Nkambou",
        });
        
        // Rôles
        int roleUtilisateurId = 2;

        Insert.IntoTable("application_roles").Row(new
        {
            name = "ADMIN",
            normalized_name = "ADMIN"
        });

        Insert.IntoTable("application_roles_users").Row(new
        {
            role_id = roleUtilisateurId,
            user_id = utilisateurGuid
        });
        
        Insert.IntoTable("informations_client").Row(new
        {
            utilisateur_id = utilisateurGuid,
            adresse = "792 rue Denise-Asselin",
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