using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models;

public class Referentiel
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Libelle { get; set; } = string.Empty;

    [Required, StringLength(40)]
    public string Niveau { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    public ICollection<Etudiant> Etudiants { get; set; } = new List<Etudiant>();
    public ICollection<Professeur> Professeurs { get; set; } = new List<Professeur>();
}
