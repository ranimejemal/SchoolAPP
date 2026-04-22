using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.Repositories;

namespace SchoolApp.Controllers;

[Authorize(Roles = "Admin")]
public class EtudiantsController(
    ApplicationDbContext context,
    IEtudiantRepository etudiantRepository) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly IEtudiantRepository _etudiantRepository = etudiantRepository;

    public async Task<IActionResult> Index() => View(await _etudiantRepository.GetAllWithDetailsAsync());

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadReferentielsAsync();
        return View("Form", new Etudiant());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Etudiant model)
    {
        if (!ModelState.IsValid)
        {
            await LoadReferentielsAsync(model.ReferentielId);
            return View("Form", model);
        }

        _context.Etudiants.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var etudiant = await _context.Etudiants.FindAsync(id);
        if (etudiant is null)
        {
            return NotFound();
        }

        await LoadReferentielsAsync(etudiant.ReferentielId);
        return View("Form", etudiant);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Etudiant model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadReferentielsAsync(model.ReferentielId);
            return View("Form", model);
        }

        _context.Etudiants.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var etudiant = await _context.Etudiants.FindAsync(id);
        if (etudiant is not null)
        {
            _context.Etudiants.Remove(etudiant);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadReferentielsAsync(int? selectedId = null)
    {
        ViewBag.Referentiels = new SelectList(
            await _context.Referentiels.OrderBy(r => r.Libelle).ToListAsync(),
            nameof(Referentiel.Id),
            nameof(Referentiel.Libelle),
            selectedId);
    }
}
