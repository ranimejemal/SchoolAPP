using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models;

public class Etudiant : Personne
{
    [Required, StringLength(20)]
    public string Matricule { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateOnly DateInscription { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Referentiel")]
    public int ReferentielId { get; set; }
    public Referentiel? Referentiel { get; set; }

    public Compte? Compte { get; set; }
    public ICollection<Absence> Absences { get; set; } = new List<Absence>();
}
