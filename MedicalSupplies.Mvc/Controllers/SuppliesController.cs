using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalSupplies.Mvc.Data;
using MedicalSupplies.Mvc.Models;
using MedicalSupplies.Mvc.ViewModels;

namespace MedicalSupplies.Mvc.Controllers;

public class SuppliesController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<SuppliesController> _logger;

    public SuppliesController(AppDbContext context, ILogger<SuppliesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var supplies = await _context.Supplies
            .Include(s => s.Category)
            .OrderByDescending(s => s.LastUpdated)
            .ToListAsync();
        return View(supplies);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? keyword, decimal? minPrice)
    {
        var query = _context.Supplies.Include(s => s.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(s => s.Name.Contains(keyword) || s.Code.Contains(keyword));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(s => s.Price >= minPrice.Value);
        }

        var supplies = await query.ToListAsync();

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
        return View(new SupplyCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SupplyCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var exists = await _context.Supplies
            .IgnoreQueryFilters()
            .AnyAsync(s => s.Code == model.Code);

        if (exists)
        {
            ModelState.AddModelError(nameof(model.Code), "Mã vật tư này đã tồn tại.");
            return View(model);
        }

        var supply = new Supply
        {
            Name = model.Name,
            Code = model.Code,
            Barcode = model.Barcode ?? "",
            Price = model.Price,
            Quantity = model.Quantity,
            Provider = model.Provider ?? "",
            SupplyCategoryId = model.SupplyCategoryId,
            LastUpdated = DateTime.Now
        };

        _context.Supplies.Add(supply);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Supply created. SupplyId={SupplyId}, Code={Code}", supply.Id, supply.Code);

        TempData["Success"] = "Đã thêm vật tư thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var supply = await _context.Supplies.FirstOrDefaultAsync(s => s.Id == id);
        if (supply == null) return NotFound();

        var model = new SupplyEditViewModel
        {
            Id = supply.Id,
            Name = supply.Name,
            Code = supply.Code,
            Barcode = supply.Barcode,
            Price = supply.Price,
            Quantity = supply.Quantity,
            Provider = supply.Provider,
            SupplyCategoryId = supply.SupplyCategoryId,
            RowVersion = supply.RowVersion == null ? "" : Convert.ToBase64String(supply.RowVersion)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SupplyEditViewModel model)
    {
        if (id != model.Id) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var supply = await _context.Supplies.FirstOrDefaultAsync(s => s.Id == id);
        if (supply == null) return NotFound();

        supply.Name = model.Name;
        supply.Code = model.Code;
        supply.Barcode = model.Barcode ?? "";
        supply.Price = model.Price;
        supply.Quantity = model.Quantity;
        supply.Provider = model.Provider ?? "";
        supply.SupplyCategoryId = model.SupplyCategoryId;
        supply.LastUpdated = DateTime.Now;

        byte[]? originalRv = string.IsNullOrEmpty(model.RowVersion) ? null : Convert.FromBase64String(model.RowVersion);
        _context.Entry(supply).Property("RowVersion").OriginalValue = originalRv;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Supply updated. SupplyId={SupplyId}", id);
            TempData["Success"] = "Đã cập nhật vật tư thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            ModelState.AddModelError(string.Empty, "Dữ liệu đã được người khác cập nhật. Vui lòng tải lại trang và thử lại.");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var supply = await _context.Supplies.FirstOrDefaultAsync(s => s.Id == id);
        if (supply == null) return NotFound();
        return View(supply);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var supply = await _context.Supplies.FirstOrDefaultAsync(s => s.Id == id);
        if (supply == null) return NotFound();

        supply.IsDeleted = true;
        supply.DeletedAt = DateTime.Now;
        supply.LastUpdated = DateTime.Now;

        await _context.SaveChangesAsync();
        _logger.LogWarning("Supply soft deleted. SupplyId={SupplyId}", id);

        TempData["Success"] = "Đã xóa mềm vật tư.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Trash()
    {
        var deletedSupplies = await _context.Supplies
            .IgnoreQueryFilters()
            .Where(s => s.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

        return View(deletedSupplies);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        var supply = await _context.Supplies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted);

        if (supply == null) return NotFound();

        supply.IsDeleted = false;
        supply.DeletedAt = null;
        supply.LastUpdated = DateTime.Now;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Supply restored. SupplyId={SupplyId}", id);

        TempData["Success"] = "Đã khôi phục vật tư.";
        return RedirectToAction(nameof(Trash));
    }
}
