using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MedicalSupplies.Mvc.Models;
using MedicalSupplies.Mvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MedicalSupplies.Mvc.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var today = DateTime.UtcNow.Date;
        var model = new MedicalSupplies.Mvc.ViewModels.DashboardViewModel
        {
            TotalProducts = _context.Supplies.IgnoreQueryFilters().Count(),
            TotalOrders = _context.Issues.Count(),
            TotalAuditLogs = _context.AuditLogs.Count(),
            AccessDeniedCountToday = _context.AuditLogs.Count(l => l.Result == "Denied" && l.CreatedAt >= today),
            SensitiveActionsToday = _context.AuditLogs.Count(l => l.Result == "Success" && (l.Action == "CreateSupply" || l.Action == "EditSupply" || l.Action == "SoftDeleteSupply" || l.Action == "RestoreSupply" || l.Action == "UploadImage" || l.Action == "AdjustStock") && l.CreatedAt >= today),
            UploadDeniedCountToday = _context.AuditLogs.Count(l => l.Action == "UploadImage" && l.Result == "Failed" && l.CreatedAt >= today)
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult StatusCode(int code)
    {
        return View(code);
    }
}
