using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers;

[Authorize(Roles = "Admin")]
public class ReferentielsController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index()
    {
        var referentiels = await _context.Referentiels
            .Include(r => r.Etudiants)
            .Include(r => r.Professeurs)
            .OrderBy(r => r.Code)
            .ToListAsync();

        return View(referentiels);
    }

    [HttpGet]
    public IActionResult Create() => View("Form", new Referentiel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Referentiel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        _context.Referentiels.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var referentiel = await _context.Referentiels.FindAsync(id);
        return referentiel is null ? NotFound() : View("Form", referentiel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Referentiel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        _context.Referentiels.Update(model);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var referentiel = await _context.Referentiels.FindAsync(id);
        if (referentiel is not null)
        {
            _context.Referentiels.Remove(referentiel);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
