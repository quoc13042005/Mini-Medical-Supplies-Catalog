using Microsoft.AspNetCore.Mvc;
using MedicalSupplies.Mvc.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalSupplies.Mvc.Controllers;

public class DataHealthController : Controller
{
    private readonly AppDbContext _context;

    public DataHealthController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var canConnect = await _context.Database.CanConnectAsync();
        var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();

        ViewBag.CanConnect = canConnect;
        ViewBag.PendingMigrations = pendingMigrations.ToList();
        ViewBag.AppliedMigrations = appliedMigrations.ToList();
        ViewBag.SupplyCount = await _context.Supplies.CountAsync();
        ViewBag.CategoryCount = await _context.SupplyCategories.CountAsync();

        return View();
    }
}
