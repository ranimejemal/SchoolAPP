using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;

namespace SchoolApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<Compte>(options)
{
    public DbSet<Personne> Personnes => Set<Personne>();
    public DbSet<Etudiant> Etudiants => Set<Etudiant>();
    public DbSet<Professeur> Professeurs => Set<Professeur>();
    public DbSet<Referentiel> Referentiels => Set<Referentiel>();
    public DbSet<Absence> Absences => Set<Absence>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Personne>()
            .HasDiscriminator<string>("TypePersonne")
            .HasValue<Etudiant>("Etudiant")
            .HasValue<Professeur>("Professeur");

        modelBuilder.Entity<Personne>()
            .Property(p => p.Nom)
            .HasMaxLength(60);

        modelBuilder.Entity<Personne>()
            .Property(p => p.Prenom)
            .HasMaxLength(60);

        modelBuilder.Entity<Personne>()
            .Property(p => p.Email)
            .HasMaxLength(120);

        modelBuilder.Entity<Referentiel>()
            .HasIndex(r => r.Code)
            .IsUnique();

        modelBuilder.Entity<Referentiel>()
            .Property(r => r.Code)
            .HasMaxLength(20);

        modelBuilder.Entity<Referentiel>()
            .Property(r => r.Libelle)
            .HasMaxLength(80);

        modelBuilder.Entity<Referentiel>()
            .Property(r => r.Niveau)
            .HasMaxLength(40);

        modelBuilder.Entity<Etudiant>()
            .HasIndex(e => e.Matricule)
            .IsUnique();

        modelBuilder.Entity<Etudiant>()
            .Property(e => e.Matricule)
            .HasMaxLength(20);

        modelBuilder.Entity<Etudiant>()
            .HasOne(e => e.Referentiel)
            .WithMany(r => r.Etudiants)
            .HasForeignKey(e => e.ReferentielId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Etudiant>()
            .Property(e => e.DateInscription)
            .HasColumnType("date");

        modelBuilder.Entity<Professeur>()
            .Property(p => p.Specialite)
            .HasMaxLength(80);

        modelBuilder.Entity<Professeur>()
            .Property(p => p.Grade)
            .HasMaxLength(50);

        modelBuilder.Entity<Professeur>()
            .HasOne(p => p.ReferentielPrincipal)
            .WithMany(r => r.Professeurs)
            .HasForeignKey(p => p.ReferentielPrincipalId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Absence>()
            .Property(a => a.Libelle)
            .HasMaxLength(100);

        modelBuilder.Entity<Absence>()
            .Property(a => a.Date)
            .HasColumnType("date");

        modelBuilder.Entity<Absence>()
            .HasOne(a => a.Etudiant)
            .WithMany(e => e.Absences)
            .HasForeignKey(a => a.EtudiantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Compte>()
            .Property(c => c.DisplayName)
            .HasMaxLength(80);

        modelBuilder.Entity<Compte>()
            .Property(c => c.Role)
            .HasMaxLength(20);

        modelBuilder.Entity<Compte>()
            .HasOne(c => c.Etudiant)
            .WithOne(e => e.Compte)
            .HasForeignKey<Compte>(c => c.EtudiantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
