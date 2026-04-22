using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models;

public class Absence
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required, StringLength(100)]
    public string Libelle { get; set; } = string.Empty;

    [Range(1, 8)]
    [Display(Name = "Duree (heures)")]
    public int DureeHeures { get; set; } = 1;

    public bool Justifiee { get; set; }

    [Display(Name = "Etudiant")]
    public int EtudiantId { get; set; }
    public Etudiant? Etudiant { get; set; }
}
