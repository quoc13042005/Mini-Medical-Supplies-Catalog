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
    public IActionResult Index(string? searchString, string? category, Models.SupplyStatus? status, int page = 1)
    {
        int pageSize = 10; // Thay đổi thành 10 theo yêu cầu
        int totalItems;

        var supplies = _supplyService.GetPagedSupplies(searchString, category, status, page, pageSize, out totalItems);
        
        var viewModels = supplies.Select(s => new SupplyListItemViewModel
        {
            Id = s.Id,
            Code = s.Code,
            Name = s.Name,
            Category = s.Category,
            Provider = s.Provider,
            Price = s.Price,
            Quantity = s.Quantity,
            Status = s.Status
        }).ToList();

        var categories = _supplyService.GetAllCategories()
            .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = c, Text = c })
            .ToList();
        categories.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "", Text = "-- Tất cả nhóm --" });

        var statusList = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
        {
            new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "", Text = "-- Tất cả trạng thái --" },
            new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = ((int)Models.SupplyStatus.InStock).ToString(), Text = "Còn hàng" },
            new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = ((int)Models.SupplyStatus.NeedsRestock).ToString(), Text = "Cần nhập thêm" },
            new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = ((int)Models.SupplyStatus.OutOfStock).ToString(), Text = "Đã hết hàng" }
        };

        var model = new SupplyIndexViewModel
        {
            Supplies = viewModels,
            SearchString = searchString,
            Category = category,
            Status = status,
            Categories = categories,
            StatusList = statusList,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            PageSize = pageSize
        };

        return View(model);
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

    [HttpGet]
    public IActionResult Search(string? keyword, decimal? minPrice)
    {
        var supplies = _supplyService.Search(keyword, minPrice)
            .Select(s => new SupplyListItemViewModel
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                Category = s.Category,
                Provider = s.Provider,
                Price = s.Price,
                Quantity = s.Quantity,
                Status = s.Status
            })
            .ToList();

        var viewModel = new SupplySearchViewModel
        {
            Keyword = keyword ?? "",
            MinPrice = minPrice,
            Supplies = supplies
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new SupplyCreateViewModel
        {
            Quantity = 1,
            MinQuantity = 1
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SupplyCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _supplyService.Create(model);

        TempData["SuccessMessage"] = "Đã thêm vật tư thành công.";

        return RedirectToAction(nameof(Index));
    }
}
