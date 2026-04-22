using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services;

namespace SchoolApp.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController(IStatisticsService statisticsService) : Controller
{
    private readonly IStatisticsService _statisticsService = statisticsService;

    public async Task<IActionResult> Index() => View(await _statisticsService.GetDashboardAsync());
}
