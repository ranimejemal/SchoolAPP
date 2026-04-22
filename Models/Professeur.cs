using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models;

public class Professeur : Personne
{
    [Required, StringLength(80)]
    public string Specialite { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Grade { get; set; } = string.Empty;

    [Display(Name = "Referentiel principal")]
    public int? ReferentielPrincipalId { get; set; }
    public Referentiel? ReferentielPrincipal { get; set; }
}
