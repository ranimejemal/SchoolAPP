using SchoolApp.ViewModels;

namespace SchoolApp.Services;

public interface IStudentPortalService
{
    Task<StudentAbsencesViewModel> GetAbsencesAsync(string userId, DateOnly? date, string? libelle);
}
