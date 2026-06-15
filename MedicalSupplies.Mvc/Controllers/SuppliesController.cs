using Microsoft.AspNetCore.Mvc;
using MedicalSupplies.Mvc.Services;
using MedicalSupplies.Mvc.ViewModels;

namespace MedicalSupplies.Mvc.Controllers;

public class SuppliesController : Controller
{
    private readonly ISupplyService _supplyService;

    public SuppliesController(ISupplyService supplyService)
    {
        _supplyService = supplyService;
    }

    public async Task<IActionResult> Index()
    {
        var supplies = await _supplyService.GetSupplyListAsync();
        return View(supplies);
    }

    public async Task<IActionResult> Filter(int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var supplies = await _supplyService.FilterSuppliesAsync(categoryId, minPrice, maxPrice);
        ViewBag.CategoryId = categoryId;
        ViewBag.MinPrice = minPrice;
        ViewBag.MaxPrice = maxPrice;
        return View(supplies);
    }
}
