using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolApp.ViewModels;

public class CompteFormViewModel
{
    public string? Id { get; set; }

    [Required, EmailAddress, Display(Name = "Login / Email")]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(80), Display(Name = "Nom affiché")]
    public string DisplayName { get; set; } = string.Empty;

    [Required, Display(Name = "Rôle")]
    public string Role { get; set; } = "User";

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Etudiant lié")]
    public int? EtudiantId { get; set; }

    public List<SelectListItem> Etudiants { get; set; } = [];
}
