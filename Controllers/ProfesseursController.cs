using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers;

[Authorize(Roles = "Admin")]
public class ProfesseursController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var professeurs = await _context.Professeurs
            .Include(p => p.ReferentielPrincipal)
            .OrderBy(p => p.Nom)
            .ToListAsync();

        return View(professeurs);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadReferentielsAsync();
        return View("Form", new Professeur());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Professeur model)
    {
        if (!ModelState.IsValid)
        {
            await LoadReferentielsAsync(model.ReferentielPrincipalId);
            return View("Form", model);
        }

        _context.Professeurs.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var professeur = await _context.Professeurs.FindAsync(id);
        if (professeur is null)
        {
            return NotFound();
        }

        await LoadReferentielsAsync(professeur.ReferentielPrincipalId);
        return View("Form", professeur);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Professeur model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadReferentielsAsync(model.ReferentielPrincipalId);
            return View("Form", model);
        }

        _context.Professeurs.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var professeur = await _context.Professeurs.FindAsync(id);
        if (professeur is not null)
        {
            _context.Professeurs.Remove(professeur);
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
