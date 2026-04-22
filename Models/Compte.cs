using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolApp.Models;

public class Compte : IdentityUser
{
    [Required, StringLength(80)]
    public string DisplayName { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Role { get; set; } = "User";

    public int? EtudiantId { get; set; }
    public Etudiant? Etudiant { get; set; }
}
