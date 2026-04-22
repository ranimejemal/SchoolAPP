using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Repositories;

namespace SchoolApp.Controllers.Api;

[ApiController]
[Route("api/absences")]
[Authorize]
public class AbsencesApiController(IAbsenceRepository absenceRepository) : ControllerBase
{
    private readonly IAbsenceRepository _absenceRepository = absenceRepository;

    [HttpGet("me")]
    public async Task<IActionResult> GetMyAbsences([FromQuery] DateOnly? date, [FromQuery] string? libelle)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        return Ok(await _absenceRepository.SearchForStudentAsync(userId, date, libelle));
    }
}
