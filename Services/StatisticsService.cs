using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.ViewModels;

namespace SchoolApp.Services;

public class StatisticsService(ApplicationDbContext context) : IStatisticsService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var justifiedAbsences = await _context.Absences.CountAsync(a => a.Justifiee);
        var totalAbsences = await _context.Absences.CountAsync();

        return new DashboardViewModel
        {
            EtudiantsCount = await _context.Etudiants.CountAsync(),
            ProfesseursCount = await _context.Professeurs.CountAsync(),
            ReferentielsCount = await _context.Referentiels.CountAsync(),
            AbsencesCount = totalAbsences,
            JustifiedRate = totalAbsences == 0 ? 0 : Math.Round((double)justifiedAbsences / totalAbsences * 100, 1),
            AbsencesByReferentiel = await _context.Absences
                .GroupBy(a => a.Etudiant!.Referentiel!.Libelle)
                .Select(group => new AbsenceStatItemViewModel
                {
                    Label = group.Key,
                    Value = group.Count()
                })
                .OrderByDescending(item => item.Value)
                .ToListAsync(),
            RecentAbsences = await _context.Absences
                .Include(a => a.Etudiant)
                .ThenInclude(e => e!.Referentiel)
                .OrderByDescending(a => a.Date)
                .Take(6)
                .Select(a => new RecentAbsenceViewModel
                {
                    Etudiant = a.Etudiant!.NomComplet,
                    Referentiel = a.Etudiant.Referentiel!.Libelle,
                    Date = a.Date,
                    Libelle = a.Libelle,
                    Justifiee = a.Justifiee
                })
                .ToListAsync()
        };
    }
}
