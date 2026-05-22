namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyStatsViewModel
{
    public int TotalCategories { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int OutOfStockCount { get; set; }
    public int NeedsRestockCount { get; set; }
}
