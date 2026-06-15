using Microsoft.AspNetCore.Mvc;
using MedicalSupplies.Mvc.Services;
using MedicalSupplies.Mvc.ViewModels;

namespace MedicalSupplies.Mvc.Controllers;

public class IssuesController : Controller
{
    private readonly IIssueService _issueService;

    public IssuesController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new IssueCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IssueCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _issueService.CreateIssueAsync(model);
            TempData["SuccessMessage"] = "Tạo phiếu xuất kho thành công.";
            return RedirectToAction("Index", "Supplies");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }
}
