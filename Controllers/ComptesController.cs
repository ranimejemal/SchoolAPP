using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using SchoolApp.ViewModels;

namespace SchoolApp.Controllers;

[Authorize(Roles = "Admin")]
public class ComptesController(
    ApplicationDbContext context,
    UserManager<Compte> userManager) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<Compte> _userManager = userManager;

    public async Task<IActionResult> Index()
    {
        var comptes = await _userManager.Users
            .Include(c => c.Etudiant)
            .OrderBy(c => c.Email)
            .ToListAsync();

        return View(comptes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CompteFormViewModel();
        await LoadEtudiantsAsync(model);
        return View("Form", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CompteFormViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.AddModelError(nameof(model.Password), "Le mot de passe est obligatoire.");
        }

        if (!ModelState.IsValid)
        {
            await LoadEtudiantsAsync(model);
            return View("Form", model);
        }

        var compte = new Compte
        {
            UserName = model.Email,
            Email = model.Email,
            DisplayName = model.DisplayName,
            Role = model.Role,
            EtudiantId = model.Role == "User" ? model.EtudiantId : null,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(compte, model.Password!);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            await LoadEtudiantsAsync(model);
            return View("Form", model);
        }

        await EnsureRoleAlignmentAsync(compte, model.Role);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var compte = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == id);
        if (compte is null)
        {
            return NotFound();
        }

        var model = new CompteFormViewModel
        {
            Id = compte.Id,
            Email = compte.Email ?? string.Empty,
            DisplayName = compte.DisplayName,
            Role = compte.Role,
            EtudiantId = compte.EtudiantId
        };

        await LoadEtudiantsAsync(model);
        return View("Form", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, CompteFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await LoadEtudiantsAsync(model);
            return View("Form", model);
        }

        var compte = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == id);
        if (compte is null)
        {
            return NotFound();
        }

        compte.UserName = model.Email;
        compte.Email = model.Email;
        compte.DisplayName = model.DisplayName;
        compte.Role = model.Role;
        compte.EtudiantId = model.Role == "User" ? model.EtudiantId : null;

        var result = await _userManager.UpdateAsync(compte);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            await LoadEtudiantsAsync(model);
            return View("Form", model);
        }

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(compte);
            await _userManager.ResetPasswordAsync(compte, resetToken, model.Password);
        }

        await EnsureRoleAlignmentAsync(compte, model.Role);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var compte = await _userManager.FindByIdAsync(id);
        if (compte is not null)
        {
            await _userManager.DeleteAsync(compte);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadEtudiantsAsync(CompteFormViewModel model)
    {
        model.Etudiants = await _context.Etudiants
            .OrderBy(e => e.Nom)
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Prenom} {e.Nom} ({e.Matricule})"
            })
            .ToListAsync();
    }

    private async Task EnsureRoleAlignmentAsync(Compte compte, string role)
    {
        foreach (var existingRole in await _userManager.GetRolesAsync(compte))
        {
            if (!string.Equals(existingRole, role, StringComparison.OrdinalIgnoreCase))
            {
                await _userManager.RemoveFromRoleAsync(compte, existingRole);
            }
        }

        if (!await _userManager.IsInRoleAsync(compte, role))
        {
            await _userManager.AddToRoleAsync(compte, role);
        }
    }
}
