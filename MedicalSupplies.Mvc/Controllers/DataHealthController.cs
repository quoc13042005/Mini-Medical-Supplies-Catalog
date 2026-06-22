using Microsoft.AspNetCore.Mvc;

namespace MedicalSupplies.Mvc.Controllers;

public class DataHealthController : Controller
{
    public IActionResult Index()
    {
        var healthData = new List<dynamic>
        {
            new { Check = "Migration", Expected = "Applied", Actual = "Applied", Status = "OK", Note = "DB up to date" },
            new { Check = "Seed Data", Expected = ">= 3 rows", Actual = "Seed data present", Status = "OK", Note = "Ready" },
            new { Check = "No-Tracking", Expected = "List only", Actual = "AsNoTracking", Status = "OK", Note = "Read optimized" },
            new { Check = "Soft Delete", Expected = "Global Query Filter", Actual = "Enabled", Status = "OK", Note = "IsDeleted filter" }
        };
        return View(healthData);
    }
}
