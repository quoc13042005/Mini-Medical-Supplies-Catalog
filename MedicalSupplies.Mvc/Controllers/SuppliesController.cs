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

    // 1. Trang hiển thị danh sách dữ liệu
    public IActionResult Index()
    {
        var supplies = _supplyService.GetAllSupplies();
        var viewModels = supplies.Select(s => new SupplyListItemViewModel
        {
            Id = s.Id,
            Code = s.Code,
            Name = s.Name,
            Category = s.Category,
            Price = s.Price,
            Status = s.Status
        }).ToList();

        return View(viewModels);
    }

    // 2. Trang xem chi tiết một đối tượng
    public IActionResult Detail(int id)
    {
        var supply = _supplyService.GetSupplyById(id);
        if (supply == null)
        {
            return NotFound("Không tìm thấy vật tư y tế này."); // 1 Action xử lý không tìm thấy dữ liệu bằng NotFound()
        }

        var viewModel = new SupplyDetailViewModel
        {
            Id = supply.Id,
            Code = supply.Code,
            Name = supply.Name,
            Category = supply.Category,
            Provider = supply.Provider,
            Price = supply.Price,
            Quantity = supply.Quantity,
            MinQuantity = supply.MinQuantity,
            Status = supply.Status,
            LastUpdated = supply.LastUpdated
        };

        return View(viewModel);
    }

    // 3. Trang thống kê tổng quan
    public IActionResult Stats()
    {
        var stats = new SupplyStatsViewModel
        {
            TotalCategories = _supplyService.GetTotalCategoriesCount(),
            TotalQuantity = _supplyService.GetTotalQuantity(),
            TotalInventoryValue = _supplyService.GetTotalInventoryValue(),
            OutOfStockCount = _supplyService.GetOutOfStockCount(),
            NeedsRestockCount = _supplyService.GetNeedsRestockCount()
        };

        return View(stats);
    }

    // 1 Action trả về text bằng Content()
    public IActionResult SupplyText()
    {
        return Content("Đây là dữ liệu dạng text đơn giản từ hệ thống quản lý vật tư y tế.");
    }

    // 1 Action trả về JSON bằng Json()
    public IActionResult SupplyJson()
    {
        var supplies = _supplyService.GetAllSupplies();
        return Json(supplies);
    }

    // 1 Action chuyển hướng bằng RedirectToAction()
    public IActionResult RedirectToCatalog()
    {
        return RedirectToAction(nameof(Index));
    }

    // 1 Action xử lý trường hợp không tìm thấy dữ liệu bằng NotFound()
    public IActionResult Force404()
    {
        return NotFound();
    }
}
