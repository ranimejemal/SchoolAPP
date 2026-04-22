using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Repositories;

public class AbsenceRepository(ApplicationDbContext context) : IAbsenceRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<Absence>> GetAllWithDetailsAsync() =>
        _context.Absences
            .Include(a => a.Etudiant)
            .ThenInclude(e => e!.Referentiel)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

    public Task<Absence?> GetByIdWithDetailsAsync(int id) =>
        _context.Absences
            .Include(a => a.Etudiant)
            .ThenInclude(e => e!.Referentiel)
            .FirstOrDefaultAsync(a => a.Id == id);

    public Task<List<Absence>> SearchForStudentAsync(string userId, DateOnly? date, string? libelle)
    {
        var query = _context.Absences
            .Include(a => a.Etudiant)
            .Where(a => a.Etudiant!.Compte != null && a.Etudiant.Compte.Id == userId);

        if (date.HasValue)
        {
            query = query.Where(a => a.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(libelle))
        {
            var term = libelle.Trim();
            query = query.Where(a => a.Libelle.Contains(term));
        }

        return query
            .OrderByDescending(a => a.Date)
            .ToListAsync();
    }
}
