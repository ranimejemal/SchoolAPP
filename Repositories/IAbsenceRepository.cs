using SchoolApp.Models;

namespace SchoolApp.Repositories;

public interface IAbsenceRepository
{
    Task<List<Absence>> GetAllWithDetailsAsync();
    Task<Absence?> GetByIdWithDetailsAsync(int id);
    Task<List<Absence>> SearchForStudentAsync(string userId, DateOnly? date, string? libelle);
}
