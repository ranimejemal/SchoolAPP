using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services;

namespace SchoolApp.Controllers;

[Authorize(Roles = "User")]
public class StudentPortalController(IStudentPortalService studentPortalService) : Controller
{
    private readonly IStudentPortalService _studentPortalService = studentPortalService;

    public async Task<IActionResult> MyAbsences(DateOnly? date, string? libelle)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var viewModel = await _studentPortalService.GetAbsencesAsync(userId, date, libelle);
        return View(viewModel);
    }
}
