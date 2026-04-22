using SchoolApp.Models;

namespace SchoolApp.ViewModels;

public class StudentAbsencesViewModel
{
    public string StudentName { get; set; } = string.Empty;
    public string Referentiel { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public string? Libelle { get; set; }
    public List<Absence> Absences { get; set; } = [];
}
