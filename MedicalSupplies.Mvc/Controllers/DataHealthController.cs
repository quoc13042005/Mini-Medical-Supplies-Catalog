using MedicalSupplies.Mvc.Data;
using MedicalSupplies.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        // 1. Total active supplies (AsNoTracking is used to optimize read-only queries)
        var totalActive = await _context.Supplies
            .AsNoTracking()
            .CountAsync();

        // 2. Total deleted supplies (Need IgnoreQueryFilters to see them)
        var totalDeleted = await _context.Supplies
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(s => s.IsDeleted)
            .CountAsync();

        // 3. Out of stock active supplies
        var outOfStock = await _context.Supplies
            .AsNoTracking()
            .Where(s => s.Quantity == 0)
            .CountAsync();

        // 4. Total categories
        var categories = await _context.SupplyCategories
            .AsNoTracking()
            .CountAsync();

        // 5. Total inventory value
        var totalValue = await _context.Supplies
            .AsNoTracking()
            .SumAsync(s => s.Price * s.Quantity);

        var model = new DataHealthDashboardViewModel
        {
            TotalActiveSupplies = totalActive,
            TotalDeletedSupplies = totalDeleted,
            TotalCategories = categories,
            OutOfStockSupplies = outOfStock,
            TotalInventoryValue = totalValue
        };

        return View(model);
    }
}
