using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Repositories;

namespace SchoolApp.Controllers;

[Authorize(Roles = "Admin")]
public class AbsencesController(
    ApplicationDbContext context,
    IAbsenceRepository absenceRepository) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAbsenceRepository _absenceRepository = absenceRepository;

    public async Task<IActionResult> Index() => View(await _absenceRepository.GetAllWithDetailsAsync());

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadEtudiantsAsync();
        return View("Form", new Absence());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Absence model)
    {
        if (!ModelState.IsValid)
        {
            await LoadEtudiantsAsync(model.EtudiantId);
            return View("Form", model);
        }

        _context.Absences.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var absence = await _context.Absences.FindAsync(id);
        if (absence is null)
        {
            return NotFound();
        }

        await LoadEtudiantsAsync(absence.EtudiantId);
        return View("Form", absence);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Absence model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadEtudiantsAsync(model.EtudiantId);
            return View("Form", model);
        }

        _context.Absences.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var absence = await _context.Absences.FindAsync(id);
        if (absence is not null)
        {
            _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadEtudiantsAsync(int? selectedId = null)
    {
        ViewBag.Etudiants = new SelectList(
            await _context.Etudiants
                .OrderBy(e => e.Nom)
                .Select(e => new { e.Id, FullName = e.Prenom + " " + e.Nom })
                .ToListAsync(),
            "Id",
            "FullName",
            selectedId);
    }
}
