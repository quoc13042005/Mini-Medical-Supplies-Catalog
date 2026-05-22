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
}
