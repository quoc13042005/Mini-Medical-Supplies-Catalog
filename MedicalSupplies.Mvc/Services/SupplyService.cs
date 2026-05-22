using MedicalSupplies.Mvc.Models;

namespace MedicalSupplies.Mvc.Services;

public class SupplyService : ISupplyService
{
    private readonly List<Supply> _supplies;

    public SupplyService()
    {
        _supplies = new List<Supply>
        {
            new Supply { Id = 1, Code = "MS001", Name = "Khẩu trang y tế 4 lớp", Category = "Vật tư tiêu hao", Provider = "MediPlast", Price = 50000, Quantity = 1000, MinQuantity = 200, LastUpdated = DateTime.Now.AddDays(-1) },
            new Supply { Id = 2, Code = "MS002", Name = "Găng tay y tế Nitrile", Category = "Vật tư tiêu hao", Provider = "VGlove", Price = 80000, Quantity = 500, MinQuantity = 100, LastUpdated = DateTime.Now.AddDays(-2) },
            new Supply { Id = 3, Code = "MS003", Name = "Bơm tiêm nhựa 5ml", Category = "Dụng cụ tiêm truyền", Provider = "Vinahoc", Price = 1500, Quantity = 0, MinQuantity = 500, LastUpdated = DateTime.Now.AddDays(-5) },
            new Supply { Id = 4, Code = "MS004", Name = "Dây truyền dịch", Category = "Dụng cụ tiêm truyền", Provider = "Danapha", Price = 8000, Quantity = 50, MinQuantity = 100, LastUpdated = DateTime.Now },
            new Supply { Id = 5, Code = "MS005", Name = "Máy đo huyết áp điện tử", Category = "Thiết bị y tế", Provider = "Omron", Price = 1200000, Quantity = 15, MinQuantity = 5, LastUpdated = DateTime.Now.AddDays(-10) }
        };
    }

    public IEnumerable<Supply> GetAllSupplies()
    {
        return _supplies;
    }

    public Supply? GetSupplyById(int id)
    {
        return _supplies.FirstOrDefault(s => s.Id == id);
    }

    public int GetTotalCategoriesCount()
    {
        return _supplies.Select(s => s.Category).Distinct().Count();
    }

    public int GetTotalQuantity()
    {
        return _supplies.Sum(s => s.Quantity);
    }

    public decimal GetTotalInventoryValue()
    {
        return _supplies.Sum(s => s.Price * s.Quantity);
    }

    public int GetOutOfStockCount()
    {
        return _supplies.Count(s => s.Status == SupplyStatus.OutOfStock);
    }

    public int GetNeedsRestockCount()
    {
        return _supplies.Count(s => s.Status == SupplyStatus.NeedsRestock);
    }
}
