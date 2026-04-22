using SchoolApp.Models;

namespace SchoolApp.Repositories;

public interface IEtudiantRepository
{
    Task<List<Etudiant>> GetAllWithDetailsAsync();
    Task<Etudiant?> GetByIdWithDetailsAsync(int id);
}
