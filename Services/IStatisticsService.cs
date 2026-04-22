using SchoolApp.ViewModels;

namespace SchoolApp.Services;

public interface IStatisticsService
{
    Task<DashboardViewModel> GetDashboardAsync();
}
