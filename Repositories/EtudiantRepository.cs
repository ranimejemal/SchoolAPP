using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Repositories;

public class EtudiantRepository(ApplicationDbContext context) : IEtudiantRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<List<Etudiant>> GetAllWithDetailsAsync() =>
        _context.Etudiants
            .Include(e => e.Referentiel)
            .Include(e => e.Absences)
            .Include(e => e.Compte)
            .OrderBy(e => e.Nom)
            .ToListAsync();

    public Task<Etudiant?> GetByIdWithDetailsAsync(int id) =>
        _context.Etudiants
            .Include(e => e.Referentiel)
            .Include(e => e.Absences)
            .Include(e => e.Compte)
            .FirstOrDefaultAsync(e => e.Id == id);
}
