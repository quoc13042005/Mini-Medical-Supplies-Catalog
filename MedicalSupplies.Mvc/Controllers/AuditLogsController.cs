using MedicalSupplies.Mvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSupplies.Mvc.Controllers;

[Authorize(Policy = "CanViewAuditLog")]
public class AuditLogsController : Controller
{
    private readonly AppDbContext _context;

    public AuditLogsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? user, string? actionName, string? result, DateTime? fromDate, DateTime? toDate)
    {
        ViewData["SearchUser"] = user;
        ViewData["SearchAction"] = actionName;
        ViewData["SearchResult"] = result;
        ViewData["SearchFromDate"] = fromDate?.ToString("yyyy-MM-dd");
        ViewData["SearchToDate"] = toDate?.ToString("yyyy-MM-dd");

        var query = _context.AuditLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(user))
        {
            query = query.Where(l => l.UserName != null && l.UserName.Contains(user));
        }
        if (!string.IsNullOrEmpty(actionName))
        {
            query = query.Where(l => l.Action.Contains(actionName));
        }
        if (!string.IsNullOrEmpty(result))
        {
            query = query.Where(l => l.Result == result);
        }
        if (fromDate.HasValue)
        {
            query = query.Where(l => l.CreatedAt >= fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query = query.Where(l => l.CreatedAt <= toDate.Value.AddDays(1).AddTicks(-1));
        }

        var logs = await query.OrderByDescending(l => l.CreatedAt).Take(100).ToListAsync();
        return View(logs);
    }
}
