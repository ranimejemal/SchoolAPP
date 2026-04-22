using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Repositories;
using SchoolApp.ViewModels;

namespace SchoolApp.Services;

public class StudentPortalService(
    ApplicationDbContext context,
    IAbsenceRepository absenceRepository) : IStudentPortalService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAbsenceRepository _absenceRepository = absenceRepository;

    public async Task<StudentAbsencesViewModel> GetAbsencesAsync(string userId, DateOnly? date, string? libelle)
    {
        var student = await _context.Etudiants
            .Include(e => e.Referentiel)
            .Include(e => e.Compte)
            .FirstAsync(e => e.Compte != null && e.Compte.Id == userId);

        var absences = await _absenceRepository.SearchForStudentAsync(userId, date, libelle);

        return new StudentAbsencesViewModel
        {
            StudentName = student.NomComplet,
            Referentiel = student.Referentiel?.Libelle ?? "Non affecte",
            Date = date,
            Libelle = libelle,
            Absences = absences
        };
    }
}
