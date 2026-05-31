using MedicalSupplies.Mvc.Models;

namespace MedicalSupplies.Mvc.Services;

public interface ISupplyService
{
    IEnumerable<Supply> GetAllSupplies();
    Supply? GetSupplyById(int id);
    int GetTotalCategoriesCount();
    int GetTotalQuantity();
    decimal GetTotalInventoryValue();
    int GetOutOfStockCount();
    int GetNeedsRestockCount();

    // Tìm kiếm, lọc và phân trang
    IEnumerable<string> GetAllCategories();
    IEnumerable<Supply> GetPagedSupplies(string? searchString, string? category, SupplyStatus? status, int page, int pageSize, out int totalItems);

    List<Supply> Search(string? keyword, decimal? minPrice);
    Supply Create(ViewModels.SupplyCreateViewModel model);
}
