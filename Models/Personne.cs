using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models;

public abstract class Personne
{
    public int Id { get; set; }

    [Required, StringLength(60)]
    public string Nom { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Prenom { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    public string NomComplet => $"{Prenom} {Nom}";
}
