using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;

namespace SchoolApp.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        UserManager<Compte> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in new[] { "Admin", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!await context.Referentiels.AnyAsync())
        {
            var referentiels = new[]
            {
                new Referentiel
                {
                    Code = "DOTNET",
                    Libelle = "Developpement .NET",
                    Niveau = "Licence 3",
                    Description = "Formation orientee web, API, EF Core et architecture logicielle."
                },
                new Referentiel
                {
                    Code = "JAVA",
                    Libelle = "Developpement Java",
                    Niveau = "Licence 2",
                    Description = "Formation backend, design patterns et programmation orientee objet."
                }
            };

            context.Referentiels.AddRange(referentiels);
            await context.SaveChangesAsync();
        }

        if (!await context.Professeurs.AnyAsync())
        {
            var dotnet = await context.Referentiels.FirstAsync(r => r.Code == "DOTNET");
            var java = await context.Referentiels.FirstAsync(r => r.Code == "JAVA");

            context.Professeurs.AddRange(
                new Professeur
                {
                    Nom = "Ben Salah",
                    Prenom = "Amina",
                    Email = "amina.bensalah@schoolapp.local",
                    Specialite = "Architecture .NET",
                    Grade = "Maitre assistante",
                    ReferentielPrincipalId = dotnet.Id
                },
                new Professeur
                {
                    Nom = "Trabelsi",
                    Prenom = "Youssef",
                    Email = "youssef.trabelsi@schoolapp.local",
                    Specialite = "Developpement Java",
                    Grade = "Assistant",
                    ReferentielPrincipalId = java.Id
                });

            await context.SaveChangesAsync();
        }

        if (!await context.Etudiants.AnyAsync())
        {
            var dotnet = await context.Referentiels.FirstAsync(r => r.Code == "DOTNET");
            var java = await context.Referentiels.FirstAsync(r => r.Code == "JAVA");

            context.Etudiants.AddRange(
                new Etudiant
                {
                    Nom = "Mansouri",
                    Prenom = "Sarra",
                    Email = "sarra@schoolapp.local",
                    Matricule = "ETU-001",
                    DateInscription = new DateOnly(2025, 9, 1),
                    ReferentielId = dotnet.Id
                },
                new Etudiant
                {
                    Nom = "Khelifi",
                    Prenom = "Omar",
                    Email = "omar@schoolapp.local",
                    Matricule = "ETU-002",
                    DateInscription = new DateOnly(2025, 9, 1),
                    ReferentielId = java.Id
                });

            await context.SaveChangesAsync();
        }

        if (!await context.Absences.AnyAsync())
        {
            var sarra = await context.Etudiants.FirstAsync(e => e.Matricule == "ETU-001");
            var omar = await context.Etudiants.FirstAsync(e => e.Matricule == "ETU-002");

            context.Absences.AddRange(
                new Absence
                {
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-7)),
                    Libelle = "Maladie",
                    Justifiee = true,
                    DureeHeures = 3,
                    EtudiantId = sarra.Id
                },
                new Absence
                {
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                    Libelle = "Retard de transport",
                    Justifiee = false,
                    DureeHeures = 2,
                    EtudiantId = sarra.Id
                },
                new Absence
                {
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                    Libelle = "Rendez-vous administratif",
                    Justifiee = true,
                    DureeHeures = 1,
                    EtudiantId = omar.Id
                });

            await context.SaveChangesAsync();
        }

        var admin = await userManager.FindByEmailAsync("admin@schoolapp.local");
        if (admin is null)
        {
            admin = new Compte
            {
                UserName = "admin@schoolapp.local",
                Email = "admin@schoolapp.local",
                DisplayName = "Administrateur principal",
                Role = "Admin",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var sarraEtudiant = await context.Etudiants.FirstAsync(e => e.Matricule == "ETU-001");
        var sarraUser = await userManager.FindByEmailAsync("sarra@schoolapp.local");
        if (sarraUser is null)
        {
            sarraUser = new Compte
            {
                UserName = "sarra@schoolapp.local",
                Email = "sarra@schoolapp.local",
                DisplayName = "Sarra Mansouri",
                Role = "User",
                EmailConfirmed = true,
                EtudiantId = sarraEtudiant.Id
            };

            await userManager.CreateAsync(sarraUser, "User123");
            await userManager.AddToRoleAsync(sarraUser, "User");
        }

        var omarEtudiant = await context.Etudiants.FirstAsync(e => e.Matricule == "ETU-002");
        var omarUser = await userManager.FindByEmailAsync("omar@schoolapp.local");
        if (omarUser is null)
        {
            omarUser = new Compte
            {
                UserName = "omar@schoolapp.local",
                Email = "omar@schoolapp.local",
                DisplayName = "Omar Khelifi",
                Role = "User",
                EmailConfirmed = true,
                EtudiantId = omarEtudiant.Id
            };

            await userManager.CreateAsync(omarUser, "User123");
            await userManager.AddToRoleAsync(omarUser, "User");
        }
    }
}
